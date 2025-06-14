using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Requests.Authentication;
using SocialMedia.Business.Services.Authentication;

namespace SocialMedia.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    { 
        _authenticationService = authenticationService;   
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LocalLoginRequest request)
    {
        try
        {
            var response = await _authenticationService.LoginLocalUserAsync(request.ToLocalLoginRequestDTO());
            var refreshToken = _authenticationService.GenerateAndSaveRefreshToken(response.Id);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
            };
            if (request.KeepMeLoggedIn)
            {
                cookieOptions.Expires = DateTime.UtcNow.AddDays(30);
            }
            
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            
            return Ok(response);
        }
        catch
        {
            return Unauthorized();
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LocalRegisterRequest request)
    {
        try
        {
            var response = await _authenticationService.RegisterLocalUser(request.ToLocalRegisterRequestDTO());
            
            return Ok(response);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return Unauthorized();

        var response = await _authenticationService.RefreshJWTToken(token);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(30)
        };
        Response.Cookies.Append("refreshToken", response.RefreshToken, cookieOptions);
        
        return Ok(new { token = response.JwtToken });
    }
}
