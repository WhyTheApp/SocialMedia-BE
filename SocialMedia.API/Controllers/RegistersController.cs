using SocialMedia.Business.Services.Example;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Requests;

namespace SocialMedia.API.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistersController : ControllerBase
{
    public IRegisterService _service;

    public RegistersController(IRegisterService exampleService)
    {
        _service = exampleService;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddRequest([FromBody] AddRegisterRequest request)
    {
        try
        {
            await _service.AddRegister(request.ToAddRegisterDTO());
            
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllRequests()
    {
        try
        {
            var response = await _service.GetAllRegisters();
            
            return Ok(response);
        }
        catch
        {
            return BadRequest();
        }
    }
}
