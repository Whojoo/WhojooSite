using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Shared.Dtos;

namespace WhojooSite.Recipes.Module.Features.Recipes.GetById;

internal class GetRecipeByIdEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapGet(
                "/recipes/{recipeId}",
                (RecipeId recipeId, IQueryHandler<GetRecipeByIdQuery, RecipeDto> queryHandler, CancellationToken cancellation) =>
                    queryHandler
                        .HandleAsync(new GetRecipeByIdQuery(recipeId), cancellation)
                        .MapToIResultAsync())
            .WithOpenApi();
    }
}