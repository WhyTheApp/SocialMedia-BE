//using SocialMedia.Data.Configurations;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;

namespace SocialMedia.Data;

public class SocialMediaDbContext : DbContext
{
    public SocialMediaDbContext(DbContextOptions<SocialMediaDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Register> Registers { get; set; }
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<FeaturedArticle> FeaturedArticles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfigurationsFromAssembly(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }
    
    private void ApplyConfigurationsFromAssembly(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var entityTypeConfigurations = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (var config in entityTypeConfigurations)
        {
            dynamic instance = Activator.CreateInstance(config);
            modelBuilder.ApplyConfiguration(instance);
        }
    }

}
