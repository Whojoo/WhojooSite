
using Scalar.AspNetCore;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using WhojooSite.Common.Cqrs.Behaviors;
using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module;
using WhojooSite.Users.Module;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .WriteTo
    .Console(
        theme: AnsiConsoleTheme.Code,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} <s:{SourceContext}>{NewLine}{Exception}",
        applyThemeToRedirectedOutput: true)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
}));

builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOutputCache();

var moduleOrchestrator = new ModuleOrchestrator(logger);

moduleOrchestrator.AddModule<RecipesModuleInitializer>();
moduleOrchestrator.AddModule<UsersModuleInitializer>();

moduleOrchestrator.ConfigureModules(builder.Services, builder.Configuration);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(moduleOrchestrator.GetModuleAssemblies());
});

builder.Services.AddMediatRLoggingBehaviors();

var app = builder.Build();

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