using Microsoft.EntityFrameworkCore;

using WhojooSite.Common;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.Module.Application.Recipes;

internal class ListHandler(RecipesDbContext recipesDbContext)
    : QueryTemplateMethod<ListHandler, ListQuery, ListQueryResult>
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    protected override async Task<Result<ListQueryResult>> HandleQueryAsync(ListQuery query)
    {
        var results = await _recipesDbContext.Recipes
            .AsNoTracking()
            .OrderBy(recipe => recipe.Id)
            .Skip(query.PageSize * query.Page)
            .Take(query.PageSize)
            .Select(recipe => new ListItem(recipe.Id, recipe.Name, recipe.Description, recipe.CookbookId))
            .ToListAsync()
            .ConfigureAwait(false);

        var totalCount = await _recipesDbContext.Recipes.CountAsync().ConfigureAwait(false);

        return Result.Success(new ListQueryResult(results, totalCount));
    }
}

internal record ListQuery(int Page, int PageSize) : ITemplateQuery<ListQueryResult>;

internal record ListQueryResult(List<ListItem> Recipes, int TotalCount);

internal record ListItem(RecipeId Id, string Name, string Description, CookbookId CookbookId);