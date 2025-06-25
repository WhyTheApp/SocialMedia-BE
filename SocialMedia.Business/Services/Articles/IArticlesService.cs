using Microsoft.AspNetCore.Http;
using SocialMedia.Business.Models.Articles;
using SocialMedia.Business.Models.PagingAndFiltering;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Services.Articles;

public interface IArticlesService
{
    Task AddArticle(AddArticleDTO request);
    Task<string> AddImage(IFormFile image);
    Task<Article> GetArticle(int articleId);
    Task<FilterResponse<Article>> GetFilteredArticles(FilterObjectDTO request);
    Task<Article> GetFeaturedArticle();
    Task<int> GetLatestArticleId();
}

