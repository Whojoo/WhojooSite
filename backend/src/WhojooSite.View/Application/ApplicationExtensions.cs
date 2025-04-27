using WhojooSite.View.Application.Recipes;

namespace WhojooSite.View.Application;

internal static class ApplicationExtensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRecipesApplication(configuration);

        return services;
    }
}