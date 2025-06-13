using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Business.Models.Authentication;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Enums;

namespace SocialMedia.Business.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    SocialMediaDbContext _dbContext;
    private IPasswordHasher<User> _hasher;
    
    public AuthenticationService(SocialMediaDbContext dbContext, IPasswordHasher<User> hasher)
    {
        _dbContext = dbContext;
        _hasher = hasher;
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
            Token = GetJWTToken(user)
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
            Email = request.Email,
            PasswordHash = String.Empty,
            RoleStatus = RoleStatus.User,
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
        
        return new LoginResponse
        {
            Id = userToAdd.Id,
            Username = userToAdd.Username,
            Token = GetJWTToken(userToAdd)
        };
    }

    private string GetJWTToken(User user)
    {
        return String.Empty;
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
}