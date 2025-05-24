using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Features.Recipes.ListComplete;

internal record ListCompleteRecipesResponse(List<Recipe> Recipes, int TotalCount);

internal class ListCompleteRecipesEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/recipes/complete", ListCompleteRecipesAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Ok<ListCompleteRecipesResponse>, NotFound>> ListCompleteRecipesAsync(
        int? pageSize,
        int? page,
        IQueryHandler<ListCompleteRecipesQuery, ListCompleteRecipesQueryResponse> queryHandler,
        CancellationToken cancellation)
    {
        var query = new ListCompleteRecipesQuery(pageSize ?? 100, page ?? 0);
        var queryResult = await queryHandler.HandleAsync(query, cancellation);

        return queryResult.Match<Results<Ok<ListCompleteRecipesResponse>, NotFound>>(
            response => TypedResults.Ok(ListCompleteRecipesMapper.MapToResponse(response)),
            _ => TypedResults.NotFound());
    }
}

[Mapper]
internal static partial class ListCompleteRecipesMapper
{
    internal static partial ListCompleteRecipesResponse MapToResponse(ListCompleteRecipesQueryResponse queryResponse);
}