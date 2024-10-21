using Microsoft.EntityFrameworkCore;

using WhojooSite.Common.Migrations;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.MigrationService;

internal class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime)
    : MigrationsWorker<RecipesDbContext>(serviceProvider, hostApplicationLifetime, ActivitySourceName)
{
    public const string ActivitySourceName = "Recipe module migrations";

    protected override async Task SeedDataAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        var recipe = new Recipe(
            new RecipeId(0),
            "Eiersalade",
            "Salade met mayo en ei",
            OwnerId.Empty,
            [],
            CookbookId.Empty,
            [],
            [],
            []);

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Recipes.AddAsync(recipe, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}