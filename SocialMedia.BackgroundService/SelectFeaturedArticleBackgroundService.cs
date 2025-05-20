using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialMedia.Data;
using SocialMedia.Data.Models;

public class SelectFeaturedArticleBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SelectFeaturedArticleBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SocialMediaDbContext>();
            
            var latestFeatured = dbContext.FeaturedArticles
                .OrderByDescending(article => article.FeaturedArticleId)
                .FirstOrDefault();

            
            if (latestFeatured != null && latestFeatured.Date.Date == DateTime.Now.Date)
                return;

            var article = dbContext.Articles.OrderByDescending(article => article.ArticleId).FirstOrDefault();
            if (article != null)
            {
                dbContext.FeaturedArticles.Add(new FeaturedArticle
                {
                    Article = article,
                    Date = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
        }
    }
}