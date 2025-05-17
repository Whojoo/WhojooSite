using Microsoft.EntityFrameworkCore;

using WhojooSite.Recipes.MigrationService;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.Services.AddDbContext<RecipesDbContext>(dbContextOptions =>
{
    dbContextOptions.UseNpgsql(
        builder.Configuration.GetConnectionString("ServerDb"),
        options => options.MigrationsHistoryTable("__EFMigrationsHistory", "recipes"));
});


var host = builder.Build();
host.Run();