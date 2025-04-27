using Microsoft.Extensions.Options;

using WhojooSite.RecipesModule.Protos.Recipes;
using WhojooSite.View.Application.Recipes;
using WhojooSite.View.Application.Recipes.Client;

namespace WhojooSite.View.Infrastructure.Recipes;

internal static class RecipesExtensions
{
    internal static IServiceCollection AddRecipesInfrastructure(this IServiceCollection services)
    {
        services.AddGrpcClient<RecipeService.RecipeServiceClient>((provider, options) =>
        {
            var recipesModuleSettingsOptions = provider.GetRequiredService<IOptions<RecipesModuleSettings>>();
            options.Address = new Uri(recipesModuleSettingsOptions.Value.GrpcUrl);
        });

        services.AddTransient<IRecipesClient, GrpcRecipesClient>();

        return services;
    }
}