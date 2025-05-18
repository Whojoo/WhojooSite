using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Shared.Dtos;

namespace WhojooSite.Recipes.Module.Features.Recipes.GetById;

internal class GetRecipeByIdEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/recipes/{recipeId:long}", GetRecipeByIdAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Ok<RecipeDto>, NotFound>> GetRecipeByIdAsync(
        long recipeId,
        IQueryHandler<GetRecipeByIdQuery, RecipeDto> queryHandler,
        CancellationToken cancellation)
    {
        var result = await queryHandler.HandleAsync(new GetRecipeByIdQuery(new RecipeId(recipeId)), cancellation);

        return result.Match<Results<Ok<RecipeDto>, NotFound>>(
            recipeDto => TypedResults.Ok(recipeDto),
            _ => TypedResults.NotFound());
    }
}