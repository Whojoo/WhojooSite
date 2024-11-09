using FastEndpoints;

using Serilog;

using WhojooSite.Common.Cqrs.Behaviors;
using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module;

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich
    .FromLogContext()
    .WriteTo
    .Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
}));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddFastEndpoints();

var moduleOrchestrator = new ModuleOrchestrator(logger);

moduleOrchestrator.AddModule<RecipesModuleInitializer>();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

moduleOrchestrator.MapModules(app);

app.UseFastEndpoints();

app.Run();