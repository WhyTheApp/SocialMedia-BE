using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Business.Models.Articles;
using SocialMedia.Business.Models.PagingAndFiltering;
using SocialMedia.Business.Services.Filtering;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Services.Articles;

public class ArticlesService: IArticlesService
{
    SocialMediaDbContext _dbContext;
    IFilterService<Article> _filterService;
    private IConfiguration _configuration;

    public ArticlesService(SocialMediaDbContext dbContext, IFilterService<Article> filterService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _filterService = filterService;
        _configuration = configuration;
    }
    
    public async Task AddArticle(AddArticleDTO request)
    {
        await _dbContext.Articles
            .AddAsync(new Article
            {
                Title = request.Title,
                Author = request.Author,
                Content = request.Content,
                Date = DateTime.UtcNow,
            });
        
        _dbContext.SaveChanges();
    }
    
    public async Task EditArticle(EditArticleDTO request)
    {
        var article = await _dbContext.Articles.FirstOrDefaultAsync(article => article.ArticleId == request.ArticleId);

        if (article == null)
            return;

        article.Title = request.Title;
        article.Content = request.Content;
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<string> AddImage(IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
            if (!allowedTypes.Contains(image.ContentType.ToLower()))
            {
                throw new Exception("Invalid image type.");
            }
            
            var uploadPath = _configuration["Uploads:Path"];
            var uploadUrl = _configuration["Uploads:URL"];

            var extension = Path.GetExtension(image.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            Directory.CreateDirectory(uploadPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            if (!uploadUrl.EndsWith("/")) uploadUrl += "/";
            
            return $"{uploadUrl}{uniqueFileName}";
        }
        
        throw new Exception("Invalid image type.");
    }


    public async Task<Article> GetArticle(int articleId)
    {
        var article = await _dbContext.Articles.Where(article => article.ArticleId == articleId).FirstOrDefaultAsync();

        if (article == null)
            throw new KeyNotFoundException(); 
                
        return article;
    }

    public async Task<FilterResponse<Article>> GetFilteredArticles(FilterObjectDTO request)
    {
        var articles = await _filterService.Filter(request, _dbContext.Articles);
        
        return articles;
    }

    public async Task<Article> GetFeaturedArticle()
    {
        var article = await _dbContext.FeaturedArticles
            .Include(article => article.Article)
            .OrderByDescending(article => article.FeaturedArticleId)
            .FirstOrDefaultAsync();

        return article.Article;
    }

    public async Task<int> GetLatestArticleId()
    {
        var articleId = _dbContext.Articles.Max(article => article.ArticleId);

        return articleId;
    }
}