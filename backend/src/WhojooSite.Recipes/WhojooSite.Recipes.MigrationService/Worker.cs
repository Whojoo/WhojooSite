using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.MigrationService;

public class Worker(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    public const string ActivitySourceName = "Migrations";

    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecipesDbContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken).ConfigureAwait(false);
            await RunMigrationAsync(dbContext, cancellationToken).ConfigureAwait(false);
            await SeedDataAsync(dbContext, cancellationToken).ConfigureAwait(false);
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
                if (!await creator.ExistsAsync(cancellationToken))
                {
                    await creator.CreateAsync(cancellationToken);
                }
            })
            .ConfigureAwait(false);
    }

    private static async Task RunMigrationAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy
            .ExecuteAsync(dbContext, async context =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                // await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                await context.Database.MigrateAsync(cancellationToken);
                // await transaction.CommitAsync(cancellationToken);
            })
            .ConfigureAwait(false);
    }

    private static Task SeedDataAsync(RecipesDbContext dbContext, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
        // SupportTicket firstTicket = new()
        // {
        //     Title = "Test Ticket",
        //     Description = "Default ticket, please ignore!",
        //     Completed = true
        // };
        //
        // var strategy = dbContext.Database.CreateExecutionStrategy();
        // await strategy.ExecuteAsync(async () =>
        // {
        //     // Seed the database
        //     await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        //     await dbContext.Tickets.AddAsync(firstTicket, cancellationToken);
        //     await dbContext.SaveChangesAsync(cancellationToken);
        //     await transaction.CommitAsync(cancellationToken);
        // });
    }
}