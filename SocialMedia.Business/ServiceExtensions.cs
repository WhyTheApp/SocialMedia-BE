using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Business.Services.Articles;
using SocialMedia.Business.Services.Filtering;
using SocialMedia.Business.Services.Registers;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>(); 
        services.AddScoped<IArticlesService, ArticlesService>(); 
        services.AddScoped<IArticlesService, ArticlesService>(); 
        services.AddScoped(typeof(IFilterService<>), typeof(FilterService<>));
    }
}
