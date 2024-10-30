using System.Reflection;

using FastEndpoints;

using WhojooSite.Common.Cqrs.Behaviors;
using WhojooSite.Recipes.Module;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddFastEndpoints();

Assembly[] moduleAssemblies =
[
    typeof(IRecipesModuleAssemblyMarker).Assembly
];

builder.ConfigureRecipesModules();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(moduleAssemblies);
});

builder.Services.AddMediatRLoggingBehaviors();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseOutputCache();

app.MapRecipesModule();

app.UseFastEndpoints();

app.Run();