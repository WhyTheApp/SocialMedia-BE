using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SocialMedia.Business.Services.Articles;
using SocialMedia.Business.Services.Authentication;
using SocialMedia.Business.Services.Filtering;
using SocialMedia.Business.Services.Registers;
using SocialMedia.Data.Models;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>(); 
        services.AddScoped<IArticlesService, ArticlesService>(); 
        services.AddScoped<IArticlesService, ArticlesService>(); 
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped(typeof(IFilterService<>), typeof(FilterService<>));
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }
}
