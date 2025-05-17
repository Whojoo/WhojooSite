using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module;
using WhojooSite.Recipes.Module.Shared;
using WhojooSite.Users.Module;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ConfigureDefaultSerilog(builder)
    .CreateLogger();

builder.AddServiceDefaults()
    .AddSerilog();

builder.Services.AddOpenApi(
    "recipes-module",
    options =>
    {
        options.AddSchemaTransformer<StronglyTypedIdSchemaTransformer>();
    });

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOutputCache();

ModuleOrchestrator moduleOrchestrator = new(logger);

moduleOrchestrator.AddModule(new RecipesModuleInitializer());
moduleOrchestrator.AddModule(new UsersModuleInitializer());

moduleOrchestrator.ConfigureModules(builder);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseOutputCache();

moduleOrchestrator.MapModules(app);

await app.RunAsync();