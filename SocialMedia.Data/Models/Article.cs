namespace SocialMedia.Data.Models;

public class Article
{
    public int ArticleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
}