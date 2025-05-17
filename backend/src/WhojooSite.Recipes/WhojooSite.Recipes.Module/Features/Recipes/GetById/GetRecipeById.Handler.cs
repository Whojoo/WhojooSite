using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Dtos;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Recipes.GetById;

internal class GetRecipeByIdHandler(
    RecipesDbContext recipesDbContext,
    ILogger<GetRecipeByIdHandler> logger)
    : RecipeModuleQueryHandler<GetRecipeByIdQuery, RecipeDto>(logger)
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    protected override async Task<Result<RecipeDto>> HandleQueryAsync(GetRecipeByIdQuery query, CancellationToken cancellation)
    {
        var recipe = await _recipesDbContext
            .Recipes
            .Where(recipe => recipe.Id == query.RecipeId)
            .Select(recipe => new RecipeDto(
                recipe.Id,
                recipe.Name,
                recipe.Description,
                recipe.CookbookId))
            .FirstOrDefaultAsync(cancellation);

        return recipe is null
            ? Result.Failure<RecipeDto>(ResultError.NotFound())
            : Result.Success(recipe);
    }
}

internal record GetRecipeByIdQuery(RecipeId RecipeId) : IQuery<RecipeDto>;