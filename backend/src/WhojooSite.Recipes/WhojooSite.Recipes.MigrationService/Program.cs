using WhojooSite.Recipes.MigrationService;
using WhojooSite.Recipes.Module.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddServiceDefaults();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<RecipesDbContext>("recipes-db");

var host = builder.Build();
host.Run();
