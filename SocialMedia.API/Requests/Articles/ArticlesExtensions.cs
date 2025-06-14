using SocialMedia.API.Requests.PagingAndFiltering;
using SocialMedia.Business.Models.Articles;
using SocialMedia.Business.Models.PagingAndFiltering;

namespace SocialMedia.API.Requests.Articles;

public static class ArticlesExtensions
{
    public static AddArticleDTO ToAddRegisterDTO(this AddArticleRequest request) =>
        new AddArticleDTO
        {
            Title = request.Title,
            Author = request.Author,
            Content = request.Content
        };
    
    public static FilterObjectDTO ToFilterObjectDTO(this FilterArticlesRequest request) => new FilterObjectDTO
    {
        OnlyCount = request.OnlyCount,
        Paging = request.Paging.toPagingDTO(),
        Filtering = request.Filtering.toFilterDTOList(),
        Sorting = request.Sorting.toSortingDTO()
    };

}