
using Scalar.AspNetCore;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module;
using WhojooSite.Users.Module;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(
        theme: AnsiConsoleTheme.Code,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
        applyThemeToRedirectedOutput: true)
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            options.ResourceAttributes.Add("service.name", builder.Configuration["OTEL_SERVICE_NAME"]!);
        })
    .CreateLogger();

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOutputCache();

var moduleOrchestrator = new ModuleOrchestrator(logger);

moduleOrchestrator.AddModule<RecipesModuleInitializer>();
moduleOrchestrator.AddModule<UsersModuleInitializer>();

moduleOrchestrator.ConfigureModules(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseOutputCache();

moduleOrchestrator.MapModules(app);

await app.RunAsync();