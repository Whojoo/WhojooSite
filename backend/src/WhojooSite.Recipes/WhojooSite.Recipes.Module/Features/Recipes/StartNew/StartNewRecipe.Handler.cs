using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Recipes.StartNew;

internal record StartNewRecipeCommand(NewRecipe NewRecipe) : ICommand<RecipeId>;

internal record NewRecipe(string Name, string Description, OwnerId OwnerId, CookbookId CookbookId);

internal class StartNewRecipeHandler(
    ILogger<RecipeModuleCommandHandler<StartNewRecipeCommand, RecipeId>> logger,
    RecipesDbContext dbContext)
    : RecipeModuleCommandHandler<StartNewRecipeCommand, RecipeId>(logger, dbContext)
{
    private readonly RecipesDbContext _dbContext = dbContext;

    protected override async Task<Result<RecipeId>> HandleCommandAsync(StartNewRecipeCommand command, CancellationToken cancellation)
    {
        return await Recipe.Factory
            .CreateSimpleRecipe(
                command.NewRecipe.Name,
                command.NewRecipe.Description,
                command.NewRecipe.CookbookId,
                command.NewRecipe.OwnerId)
            .MapAsync(recipe => StoreRecipeAsync(recipe, cancellation));
    }

    private async Task<RecipeId> StoreRecipeAsync(Recipe recipe, CancellationToken cancellation)
    {
        await _dbContext.Recipes.AddAsync(recipe, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return recipe.Id;
    }
}