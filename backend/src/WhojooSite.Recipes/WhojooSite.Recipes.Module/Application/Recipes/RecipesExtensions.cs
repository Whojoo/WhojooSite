using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Recipes.Module.Application.Recipes;

internal static class RecipesExtensions
{
    internal static IServiceCollection AddRecipeHandlers(this IServiceCollection services)
    {
        services.AddTransient<GetByIdHandler>();
        services.AddTransient<ListHandler>();

        return services;
    }
}