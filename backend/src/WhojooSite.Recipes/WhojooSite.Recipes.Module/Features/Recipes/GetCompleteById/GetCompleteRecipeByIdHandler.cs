using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Recipes.GetCompleteById;

internal record GetCompleteRecipeByIdQuery(RecipeId RecipeId) : IQuery<GetCompleteRecipeByIdResponse>;

internal record GetCompleteRecipeByIdResponse(Recipe Recipe);

internal class GetCompleteRecipeByIdHandler(
    ILogger<RecipeModuleQueryHandler<GetCompleteRecipeByIdQuery, GetCompleteRecipeByIdResponse>> logger,
    RecipesDbContext dbContext,
    IValidator<GetCompleteRecipeByIdQuery>? validator = null)
    : RecipeModuleQueryHandler<GetCompleteRecipeByIdQuery, GetCompleteRecipeByIdResponse>(logger, validator)
{
    private readonly RecipesDbContext _dbContext = dbContext;

    protected override async Task<Result<GetCompleteRecipeByIdResponse>> HandleQueryAsync(
        GetCompleteRecipeByIdQuery query,
        CancellationToken cancellation)
    {
        var recipe = await _dbContext
            .Recipes
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(recipe => recipe.Id == query.RecipeId, cancellation);

        return recipe is null
            ? Result.Failure<GetCompleteRecipeByIdResponse>(ResultError.NotFound())
            : Result.Success(new GetCompleteRecipeByIdResponse(recipe));
    }
}