using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Features.Cookbooks.List;

internal class ListCookbooksEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet("/cookbooks", ListCookbooksAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Ok<ListCookbooksResponse>, ValidationProblem>> ListCookbooksAsync(
        int? pageSize,
        long? nextKey,
        IQueryHandler<ListCookbooksQuery, ListCookbooksResponse> queryHandler,
        CancellationToken cancellation)
    {
        var query = new ListCookbooksQuery(pageSize ?? 10, nextKey);
        var queryResult = await queryHandler.HandleAsync(query, cancellation);

        return queryResult.Match<Results<Ok<ListCookbooksResponse>, ValidationProblem>>(
            response => TypedResults.Ok(response),
            errors => errors.MapToValidationProblem());
    }
}