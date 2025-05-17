using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Dtos;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Recipes.List;

internal record ListRecipesQuery(int PageSize, long? NextKey) : IQuery<ListRecipesResponse>;

internal record ListRecipesResponse(List<RecipeDto> Recipes, long NextKey);

internal sealed class ListRecipesHandler(
    ILogger<RecipeModuleQueryHandler<ListRecipesQuery, ListRecipesResponse>> logger,
    IValidator<ListRecipesQuery> validator,
    RecipesDbContext recipesDbContext)
    : RecipeModuleQueryHandler<ListRecipesQuery, ListRecipesResponse>(logger, validator)
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    protected override async Task<Result<ListRecipesResponse>> HandleQueryAsync(ListRecipesQuery query, CancellationToken cancellation)
    {
        RecipeId idCursor = new(query.NextKey ?? 0);

        var recipeList = await _recipesDbContext
            .Recipes
            .Where(recipe => recipe.Id > idCursor)
            .OrderBy(recipe => recipe.Id)
            .Take(query.PageSize)
            .Select(recipe => new RecipeDto(recipe.Id, recipe.Name, recipe.Description, recipe.CookbookId))
            .ToListAsync(cancellation)
            .ConfigureAwait(false);

        var nextKey = recipeList.Count > 0 ? recipeList[^1].Id.Value : idCursor.Value;

        return Result.Success(new ListRecipesResponse(recipeList, nextKey));
    }
}