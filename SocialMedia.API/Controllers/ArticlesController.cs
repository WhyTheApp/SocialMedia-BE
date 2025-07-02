using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> AddRequest([FromBody] AddArticleRequest request)
    {
        try
        {
            await _service.AddArticle(request.ToAddArticleDTO());
            
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [HttpPost("edit")]
    public async Task<IActionResult> EditRequest([FromBody] EditArticleRequest request)
    {
        try
        {
            await _service.EditArticle(request.ToEditArticleDTO());
            
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpPost("add-image")]
    public async Task<IActionResult> AddRequest([FromForm] IFormFile image)
    {
        try
        {
            var imageLink = await _service.AddImage(image);
            
            return Ok(imageLink);
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpGet("get-article")]
    public async Task<IActionResult> GetRequest([FromQuery] int? articleId, [FromQuery] string? slug)
    {
        try
        {
            if (articleId == null && string.IsNullOrWhiteSpace(slug))
                return BadRequest("Either articleId or slug must be provided.");
            
            var article = articleId != null
                ? await _service.GetArticleById(articleId.Value)
                : await _service.GetArticleBySlug(slug!);
            
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
    
    [HttpGet("get-slug-map")]
    public async Task<IActionResult> GetSlugMap()
    {
        try
        {
            var article = await _service.GetSlugMap();
            
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
