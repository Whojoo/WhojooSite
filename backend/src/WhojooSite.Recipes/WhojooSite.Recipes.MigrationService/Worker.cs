using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.MigrationService;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken).ConfigureAwait(false);
            await RunMigrationAsync(dbContext, cancellationToken).ConfigureAwait(false);
            // await SeedDataAsync(dbContext, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy
            .ExecuteAsync(dbCreator, async creator =>
            {
                // Create the database if it does not exist.
                // Do this first so there is then a database to start a transaction against.
                if (!await creator.ExistsAsync(cancellationToken).ConfigureAwait(false))
                {
                    await creator.CreateAsync(cancellationToken).ConfigureAwait(false);
                }
            })
            .ConfigureAwait(false);
    }

    private static async Task RunMigrationAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy
            .ExecuteAsync(dbContext, context => context.Database.MigrateAsync(cancellationToken))
            .ConfigureAwait(false);
    }

    private static async Task SeedDataAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        var ownerId = OwnerId.New();
        Cookbook cookbook = new("Basis kookboek", []);
        await dbContext.Cookbooks.AddAsync(cookbook, cancellationToken).ConfigureAwait(false);

        Recipe recipe1 = new(
            "Een pan pasta",
            "Een makkelijke pasta dat volledig in een enkele pan wordt bereid",
            ownerId,
            [
                new Step(StepId.Empty, "groente snijden", "Snijd alle groenten", RecipeId.Empty),
                new Step(StepId.Empty, "Groente bakken", "Fruit alle groenten in de pan", RecipeId.Empty),
                new Step(StepId.Empty, "Voeg de saus toe", "Voeg de saus toe", RecipeId.Empty),
                new Step(StepId.Empty, "Pasta", "Voeg de pasta toe en laat het 15 min koken", RecipeId.Empty)
            ],
            cookbook.Id,
            [],
            [],
            []);

        await dbContext.Recipes.AddAsync(recipe1, cancellationToken).ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}