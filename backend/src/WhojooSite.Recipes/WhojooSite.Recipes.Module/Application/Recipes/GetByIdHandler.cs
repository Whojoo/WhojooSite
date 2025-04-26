using Microsoft.EntityFrameworkCore;

using WhojooSite.Common;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.Module.Application.Recipes;

internal class GetByIdHandler(RecipesDbContext recipesDbContext) : QueryTemplateMethod<GetByIdHandler, GetByIdQuery, GetByIdQueryResult>
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    protected override async Task<Result<GetByIdQueryResult>> HandleQueryAsync(GetByIdQuery query)
    {
        var recipe = await _recipesDbContext
            .Recipes
            .Where(recipe => recipe.Id == query.RecipeId)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        return recipe is null
            ? Result.Failure<GetByIdQueryResult>()
            : Result.Success(new GetByIdQueryResult(recipe));
    }
}

internal record GetByIdQuery(RecipeId RecipeId) : ITemplateQuery<GetByIdQueryResult>;

internal record GetByIdQueryResult(Recipe Recipe);