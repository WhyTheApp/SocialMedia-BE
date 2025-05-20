using Microsoft.EntityFrameworkCore;
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

    public ArticlesService(SocialMediaDbContext dbContext, IFilterService<Article> filterService)
    {
        _dbContext = dbContext;
        _filterService = filterService;
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
}