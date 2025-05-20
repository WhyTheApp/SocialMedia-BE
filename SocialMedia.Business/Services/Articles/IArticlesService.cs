using SocialMedia.Business.Models.Articles;
using SocialMedia.Business.Models.PagingAndFiltering;
using SocialMedia.Data.Models;

namespace SocialMedia.Business.Services.Articles;

public interface IArticlesService
{
    Task AddArticle(AddArticleDTO request);
    Task<Article> GetArticle(int articleId);
    Task<FilterResponse<Article>> GetFilteredArticles(FilterObjectDTO request);
    Task<Article> GetFeaturedArticle();
}

