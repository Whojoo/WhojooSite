using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Features.Recipes.List;

internal sealed class ListRecipesEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet(
                "/recipes",
                (
                        int? pageSize,
                        long? nextKey,
                        IQueryHandler<ListRecipesQuery, ListRecipesResponse> queryHandler,
                        CancellationToken cancellation) =>
                    queryHandler
                        .HandleAsync(new ListRecipesQuery(pageSize ?? 10, nextKey), cancellation)
                        .MapToIResultAsync())
            .WithOpenApi();
    }
}