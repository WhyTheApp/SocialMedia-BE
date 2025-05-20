namespace SocialMedia.Data.Models;

public class FeaturedArticle
{
    public int FeaturedArticleId { get; set; }
    public Article Article { get; set; }
    public DateTime Date { get; set; }
}