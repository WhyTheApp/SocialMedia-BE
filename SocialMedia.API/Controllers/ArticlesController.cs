using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.API.Requests.Articles;
using SocialMedia.Business.Services.Articles;
using SocialMedia.Business.Services.Authentication;

namespace SocialMedia.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    public IArticlesService _service;
    private IConfiguration configuration;

    public ArticlesController(IArticlesService exampleService, IConfiguration configuration)
    {
        _service = exampleService;
        this.configuration = configuration;
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost("add")]
    public async Task<IActionResult> AddRequest([FromBody] AddArticleRequest request)
    {
        try
        {
            await _service.AddArticle(request.ToAddRegisterDTO());
            
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("get-article")]
    public async Task<IActionResult> GetRequest([FromQuery] int articleId)
    {
        try
        {
            var article = await _service.GetArticle(articleId);
            
            return Ok(article);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("get-latest-article-id")]
    public async Task<IActionResult> GetLatestArticleId()
    {
        await new EmailerService(configuration).SendVerificationEmailAsync("Catalin Catalin", " letstalk@whythe.app", "2233");
        try
        {
            var article = await _service.GetLatestArticleId();
            
            return Ok(article);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost("get-filtered")]
    public async Task<IActionResult> GetFilteredArticles([FromBody] FilterArticlesRequest request)
    {
        try
        {
            var articles = await _service.GetFilteredArticles(request.ToFilterObjectDTO());
            
            return Ok(articles);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("get-featured")]
    public async Task<IActionResult> GetFeaturedArticle()
    {
        try
        {
            var article = await _service.GetFeaturedArticle();
            
            return Ok(article);
        }
        catch
        {
            return BadRequest();
        }
    }
}
