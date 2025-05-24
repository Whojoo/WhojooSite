using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Recipes.ListComplete;

internal record ListCompleteRecipesQuery(int? PageSize, int? Page) : IQuery<ListCompleteRecipesQueryResponse>;

internal record ListCompleteRecipesQueryResponse(List<Recipe> Recipes, int TotalCount);

internal class ListCompleteRecipesHandler(
    ILogger<RecipeModuleQueryHandler<ListCompleteRecipesQuery, ListCompleteRecipesQueryResponse>> logger,
    RecipesDbContext dbContext,
    IValidator<ListCompleteRecipesQuery>? validator = null)
    : RecipeModuleQueryHandler<ListCompleteRecipesQuery, ListCompleteRecipesQueryResponse>(logger, validator)
{
    private readonly RecipesDbContext _dbContext = dbContext;

    protected override async Task<Result<ListCompleteRecipesQueryResponse>> HandleQueryAsync(
        ListCompleteRecipesQuery query,
        CancellationToken cancellation)
    {
        var pageSize = GetNormalizedPageSize(query.PageSize);
        var page = GetNormalizedPage(query.Page);

        var recipes = await _dbContext
            .Recipes
            .AsNoTracking()
            .AsSplitQuery()
            .OrderBy(recipe => recipe.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellation);

        var totalCount = await _dbContext.Recipes.CountAsync(cancellation);

        return Result<ListCompleteRecipesQueryResponse>.Success(new ListCompleteRecipesQueryResponse(recipes, totalCount));
    }

    private static int GetNormalizedPageSize(int? pageSize)
    {
        const int defaultPageSize = 100;
        const int maxPageSize = defaultPageSize;
        const int minPageSize = 10;

        var nonNullPageSize = pageSize ?? defaultPageSize;
        return Math.Min(Math.Max(nonNullPageSize, minPageSize), maxPageSize);
    }

    private static int GetNormalizedPage(int? page)
    {
        const int defaultPage = 1;

        var nonNullPage = page ?? defaultPage;
        return Math.Max(nonNullPage, defaultPage);
    }
}