using WhojooSite.Recipes.MigrationService;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<RecipesDbContext>("ServerDb");

var host = builder.Build();
host.Run();