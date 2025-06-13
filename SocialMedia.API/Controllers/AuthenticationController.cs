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
}
