using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Recipes.Module.Features.Recipes;

namespace WhojooSite.Recipes.Module.Features;

internal static class FeatureExtensions
{
    internal static void AddFeatures(this IServiceCollection services)
    {
        services.AddRecipeServices();
    }

    internal static void MapFeatures(this RouteGroupBuilder group)
    {
        group.MapRecipeEndpoints();
    }
}