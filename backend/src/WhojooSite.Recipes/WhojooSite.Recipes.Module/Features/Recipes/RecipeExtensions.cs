using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal static class RecipeExtensions
{
    internal static void AddRecipeServices(this IServiceCollection services)
    {
        ListRecipesEndpoint.AddEndpoint(services);
    }
    
    internal static void MapRecipeEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        var recipeGroup = routeGroupBuilder.MapGroup("/recipes/");
        GetRecipeByIdEndpoint.MapEndpoint(recipeGroup);
        ListRecipesEndpoint.MapEndpoint(recipeGroup);
    }
}