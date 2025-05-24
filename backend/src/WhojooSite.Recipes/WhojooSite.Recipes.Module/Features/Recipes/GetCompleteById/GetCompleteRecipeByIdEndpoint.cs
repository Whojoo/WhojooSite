using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Features.Recipes.GetCompleteById;

internal class GetCompleteRecipeByIdEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/recipes/complete/{recipeId:long}", GetCompleteRecipeByIdAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Ok<Recipe>, NotFound>> GetCompleteRecipeByIdAsync(
        long recipeId,
        IQueryHandler<GetCompleteRecipeByIdQuery, GetCompleteRecipeByIdResponse> queryHandler,
        CancellationToken cancellation)
    {
        var result = await queryHandler.HandleAsync(new GetCompleteRecipeByIdQuery(new RecipeId(recipeId)), cancellation);

        return result.Match<Results<Ok<Recipe>, NotFound>>(
            response => TypedResults.Ok(response.Recipe),
            _ => TypedResults.NotFound());
    }
}