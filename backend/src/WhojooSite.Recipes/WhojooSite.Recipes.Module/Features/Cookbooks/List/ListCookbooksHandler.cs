using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Cookbooks.List;

internal record ListCookbooksQuery(int PageSize, long? NextKey) : IQuery<ListCookbooksResponse>;

internal record ListCookbooksResponse(List<ListCookbooksItem> Cookbooks, long NextKey, int TotalCount);

internal record ListCookbooksItem(CookbookId CookbookId, string Name);

internal class ListCookbooksHandler(
    ILogger<RecipeModuleQueryHandler<ListCookbooksQuery, ListCookbooksResponse>> logger,
    RecipesDbContext recipesDbContext,
    IValidator<ListCookbooksQuery>? validator = null)
    : RecipeModuleQueryHandler<ListCookbooksQuery, ListCookbooksResponse>(logger, validator)
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    protected override async Task<Result<ListCookbooksResponse>> HandleQueryAsync(ListCookbooksQuery query, CancellationToken cancellation)
    {
        var idCursor = new CookbookId(query.NextKey ?? 0);

        var cookbooks = await _recipesDbContext
            .Cookbooks
            .Where(cookbook => cookbook.Id > idCursor)
            .OrderBy(cookbook => cookbook.Id)
            .Take(query.PageSize)
            .Select(cookbook => new ListCookbooksItem(cookbook.Id, cookbook.Name))
            .ToListAsync(cancellation);

        var totalCount = await _recipesDbContext.Cookbooks.CountAsync(cancellation);

        var nextKey = cookbooks.Count > 0 ? cookbooks[^1].CookbookId.Value : idCursor.Value;

        return Result<ListCookbooksResponse>.Success(new ListCookbooksResponse(cookbooks, nextKey, totalCount));
    }
}

internal class ListCookbooksHandlerValidator : AbstractValidator<ListCookbooksQuery>
{
    public ListCookbooksHandlerValidator()
    {
        RuleFor(query => query.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(query => query.NextKey)
            .GreaterThanOrEqualTo(0)
            .When(query => query.NextKey.HasValue);
    }
}