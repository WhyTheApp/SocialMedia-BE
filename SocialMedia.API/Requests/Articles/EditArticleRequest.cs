namespace SocialMedia.API.Requests.Articles;

public class EditArticleRequest
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}