using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Features.Recipes.List;

internal sealed class ListRecipesEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/recipes", ListRecipesAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Ok<ListRecipesResponse>, ValidationProblem>> ListRecipesAsync(
        int? pageSize,
        long? nextKey,
        IQueryHandler<ListRecipesQuery, ListRecipesResponse> queryHandler,
        CancellationToken cancellation)
    {
        var result = await queryHandler.HandleAsync(new ListRecipesQuery(pageSize ?? 10, nextKey), cancellation);

        return result.Match<Results<Ok<ListRecipesResponse>, ValidationProblem>>(
            listRecipesResponse => TypedResults.Ok(listRecipesResponse),
            errors => errors.MapToValidationProblem());
    }
}