using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Configurations;

public class FeaturedArticlesConfiguration: IEntityTypeConfiguration<FeaturedArticle>
{
    public void Configure(EntityTypeBuilder<FeaturedArticle> builder)
    {
        builder.HasKey(article => article.FeaturedArticleId);
        builder.HasIndex(article => article.Date);
    }
}
