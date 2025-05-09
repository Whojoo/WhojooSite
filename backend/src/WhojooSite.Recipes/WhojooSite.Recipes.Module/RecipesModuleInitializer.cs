using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module.Application;
using WhojooSite.Recipes.Module.Features;
using WhojooSite.Recipes.Module.Grpc;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.Module;

public class RecipesModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Recipes";

    public void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger)
    {
        applicationBuilder.AddNpgsqlDbContext<RecipesDbContext>("ServerDb");

        applicationBuilder.Services.AddFeatures();
        applicationBuilder.Services.AddApplication();
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        var recipeModuleGroup = routeGroupBuilder.MapGroup("/recipes-module");

        recipeModuleGroup.MapFeatures();
    }

    public void MapModule(WebApplication app)
    {
        app.MapGrpcServices();
    }
}