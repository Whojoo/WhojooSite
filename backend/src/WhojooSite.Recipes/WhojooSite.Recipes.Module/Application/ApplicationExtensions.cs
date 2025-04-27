using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Recipes.Module.Application.Recipes;

namespace WhojooSite.Recipes.Module.Application;

internal static class ApplicationExtensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddRecipeHandlers();

        return services;
    }
}