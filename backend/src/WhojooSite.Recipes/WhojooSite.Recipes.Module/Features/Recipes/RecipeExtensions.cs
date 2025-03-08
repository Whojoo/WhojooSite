using Microsoft.AspNetCore.Routing;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal static class RecipeExtensions
{
    internal static void MapRecipeEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        GetRecipeByIdEndpoint.MapEndpoint(routeGroupBuilder);
        ListRecipesEndpoint.MapEndpoint(routeGroupBuilder);
    }
}