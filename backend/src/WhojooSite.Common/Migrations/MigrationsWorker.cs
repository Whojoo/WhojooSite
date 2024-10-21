using System.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Trace;

namespace WhojooSite.Common.Migrations;

public abstract class MigrationsWorker<TDbContextType>(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    string activitySourceName)
    : BackgroundService
    where TDbContextType : DbContext
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;

    private readonly string _activitySourceName = activitySourceName;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var activitySource = new ActivitySource(_activitySourceName);
        using var activity = activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContextType>();
            await EnsureDatabaseAsync(dbContext, stoppingToken);
            await RunMigrationAsync(dbContext, stoppingToken);
            await SeedDataAsync(dbContext, stoppingToken);
        }
        catch (Exception e)
        {
            activity?.RecordException(e);
            throw;
        }

        _hostApplicationLifetime.StopApplication();
    }

    protected abstract Task SeedDataAsync(TDbContextType dbContext, CancellationToken cancellationToken);

    private static async Task EnsureDatabaseAsync(TDbContextType dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(TDbContextType dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}