using SocialMedia.Business.Services.Example;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterService, RegisterService>(); 
    }
}
