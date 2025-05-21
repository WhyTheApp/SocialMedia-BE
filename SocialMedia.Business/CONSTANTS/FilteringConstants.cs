using SocialMedia.Business.Models.Articles;

namespace SocialMedia.Business.Constants;

public static class FilteringConstants
{
    public static Dictionary<string,string> MapAcronymsToOperations = new Dictionary<string,string>()
    {
        {"eq", "=="},
        {"gt",">"},
        {"lt","<"},
        {"ge", ">="},
        {"le", "<="},
        {"ne", "!="},
        {"in", "in"},
        {"notin", "notin"}
    };
    
    public static readonly List<string> ValidSortingOrder = new List<string>
    {
        "asc",
        "desc",
        ""
    }; 
    
    public static readonly List<string> ValidArticlesSortingFields = new List<string>
    {
        nameof(ArticleDTO.ArticleId).ToLower(),
        nameof(ArticleDTO.Title).ToLower(),
        nameof(ArticleDTO.Date).ToLower(),
    }; 
    
    public static readonly List<string> ValidArticlesFilteringFields = new List<string>
    {
        nameof(ArticleDTO.ArticleId).ToLower(),
        nameof(ArticleDTO.Title).ToLower(),
        nameof(ArticleDTO.Date).ToLower(),
    }; 
}