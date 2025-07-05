using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Business.Helpers;
using SocialMedia.Business.Models.Authentication;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Enums;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace SocialMedia.Business.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    SocialMediaDbContext _dbContext;
    private IPasswordHasher<User> _hasher;
    private IConfiguration _configuration;
    private IEmailerService _emailerService;
    private IUsernameUniquenessChecker _uniquenessChecker;
    private IHttpClientFactory _httpClientFactory;

    
    public AuthenticationService(SocialMediaDbContext dbContext, IPasswordHasher<User> hasher, IConfiguration configuration, IEmailerService emailerService, IUsernameUniquenessChecker uniquenessChecker, IHttpClientFactory httpClientFactory)
    {
        _dbContext = dbContext;
        _hasher = hasher;
        _configuration = configuration;
        _emailerService = emailerService;
        _httpClientFactory = httpClientFactory;
        _uniquenessChecker = uniquenessChecker;
    }
    
    public async Task<LoginResponse> LoginLocalUserAsync(LocalLoginRequestDTO request)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user =>
                user.Email == request.EmailOrUsername ||
                user.Username == request.EmailOrUsername);

        if (user == null)
            throw new ArgumentException("Invalid email/username or password.");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result != PasswordVerificationResult.Success)
            throw new ArgumentException("Invalid email/username or password.");

        user.LastLoginAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = GenerateJwtToken(user)
        };
    }
    
    public async Task<LoginResponse> RegisterLocalUser(LocalRegisterRequestDTO request)
    {
        if(await IsUserAlreadyExistent(request.Email, request.Username))
            throw new InvalidOperationException();

        var uniqueId = await GetUniqueId();

        var userToAdd = new User
        {
            Id = uniqueId,
            Username = request.Username,
            Name = request.Name,
            Email = request.Email,
            PasswordHash = String.Empty,
            RoleStatus = RoleStatus.Unverified,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow,
            IsEmailConfirmed = false,
            Provider = LoginProvider.Local,
            ProviderId = String.Empty,
        };
        
        userToAdd.PasswordHash = _hasher.HashPassword(userToAdd, request.Password);
        
        await _dbContext.Users
            .AddAsync(userToAdd);
        await _dbContext.SaveChangesAsync();
        
        await SendVerificationCodeAsync(userToAdd.Id, userToAdd.Username, userToAdd.Email);

        
        return new LoginResponse
        {
            Id = userToAdd.Id,
            Username = userToAdd.Username,
            Token = GenerateJwtToken(userToAdd)
        };
    }
    
    public string GenerateAndSaveRefreshToken(Guid userId, int size = 32)
    {
        var randomNumber = new byte[size];
        
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(randomNumber);
        }
        
        var refreshToken = Convert.ToBase64String(randomNumber)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
            
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(30),
            IsRevoked = false,
            UserId = userId
        };
        _dbContext.RefreshTokens.Add(refreshTokenEntity);
        _dbContext.SaveChanges();

        return refreshToken;
    }

    public async Task<RefreshTokenResponse> RefreshJWTToken(string refreshToken)
    {
        var validRefreshToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.Expires > DateTime.UtcNow);
        
        if(validRefreshToken == null)
            throw new InvalidOperationException();
        
        validRefreshToken.IsRevoked = true;
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == validRefreshToken.UserId);

        return new RefreshTokenResponse
        {
            Token = GenerateJwtToken(user),
            RefreshToken = GenerateAndSaveRefreshToken(user.Id)
        };
    }

    public async Task<LoginResponse> VerifyEmail(VerifyEmailRequestDTO request)
    {
        var mailVerification = await _dbContext.MailVerifications.FirstOrDefaultAsync(mailVerification =>
            mailVerification.UserId == request.UserId && mailVerification.Expires > DateTime.UtcNow);

        if(mailVerification == null)
            throw new InvalidOperationException();
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == mailVerification.UserId);
        user.RoleStatus = RoleStatus.User;

        _dbContext.MailVerifications.Remove(mailVerification);
        await _dbContext.SaveChangesAsync();

        return new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = GenerateJwtToken(user),
        };
    }

    public async Task<LoginResponse> GoogleLogin(OauthRequestDTO request)
    {
        var client = _httpClientFactory.CreateClient();

        var parameters = new Dictionary<string, string>
        {
            ["client_id"] = _configuration["Google:ClientId"],
            ["client_secret"] = _configuration["Google:ClientSecret"],
            ["code"] = request.Code,
            ["code_verifier"] = request.Verifier,
            ["redirect_uri"] = request.RedirectUri,
            ["grant_type"] = "authorization_code",
        };

        var content = new FormUrlEncodedContent(parameters);
        var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);

        if (!response.IsSuccessStatusCode)
            throw new ArgumentException("Invalid oauth");

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);
        var principal = await ValidateGoogleToken(tokenResponse.IdToken);

        if (principal == null)
            throw new SecurityTokenException("Google token invalid");

        var email = principal.FindFirstValue(ClaimTypes.Email);
        var name = principal.FindFirstValue("name");
        var googleId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await GetOauthUser(email, name, googleId);

        return new LoginResponse
        {
            Id = user.Id,
            Username = user.Username,
            Token = GenerateJwtToken(user),
        };
    }

    private async Task<User> GetOauthUser(string email, string name, string providerId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
            u.Provider == LoginProvider.Google && u.ProviderId == providerId);

        if (user == null)
        {
            user = new User
            {
                Id = await GetUniqueId(),
                Email = email,
                Name = name,
                Username = GenerateUniqueUsername(name),
                PasswordHash = String.Empty,
                Provider = LoginProvider.Google,
                ProviderId = providerId,
                IsEmailConfirmed = true,
                RoleStatus = RoleStatus.User,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            await _dbContext.Users.AddAsync(user);
        }
        else
        {
            user.LastLoginAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        return user;
    }

    private string GenerateUniqueUsername(string name)
    {
        var username = String.Empty;
        
        do
        {
            var random_string = Guid.NewGuid().ToString("N")[..4];
            username = name.ToLower() + "_" + random_string;

            username = Regex.Replace(username, @"[^a-z0-9_]", "_");

            if (!char.IsLetter(username.First()))
                username = "u_" + username;

            username = username.Length > 30 ? username.Substring(0, 30) : username;
        } while (!_uniquenessChecker.IsUnique(username));
        
        return username;
    }

    private async Task<ClaimsPrincipal?> ValidateGoogleToken(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Google:ClientId"] }
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, payload.Email),
                new Claim("name", payload.Name),
                new Claim(ClaimTypes.NameIdentifier, payload.Subject)
            };

            var identity = new ClaimsIdentity(claims, "Google");
            return new ClaimsPrincipal(identity);
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }

    
    private async Task SendVerificationCodeAsync(Guid userId, string username, string email)
    {
        var random = new Random();
        string code = random.Next(1000, 10000).ToString();
      
        await _emailerService.SendVerificationEmailAsync(username, email, code);

        await _dbContext.MailVerifications.AddAsync(new MailVerification
        {
            Expires = DateTime.UtcNow + TimeSpan.FromMinutes(30),
            UserId = userId,
            Code = code
        });
        await _dbContext.SaveChangesAsync();
    }
    
    private async Task<bool> IsUserAlreadyExistent(string email, string username)
    {
        return await _dbContext
            .Users
            .AnyAsync(user => user.Email == email || user.Username == username);
    }

    private async Task<Guid> GetUniqueId()
    {
        var uniqueId = Guid.NewGuid();
        var idExists = await _dbContext
            .Users
            .AnyAsync(user => user.Id == uniqueId);
        
        while (idExists)
        {
            uniqueId = Guid.NewGuid();
            idExists = await _dbContext
                .Users
                .AnyAsync(user => user.Id == uniqueId);
        }
        
        return uniqueId;
    }
    
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim("username", user.Username), 
            new Claim("name", user.Name), 
            new Claim("role", user.RoleStatus.ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
            new Claim(JwtRegisteredClaimNames.Iat, 
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Exp,
                new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}