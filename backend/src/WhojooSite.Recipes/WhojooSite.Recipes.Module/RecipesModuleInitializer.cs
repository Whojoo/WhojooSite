using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module.Features;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module;

public class RecipesModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Recipes";

    public void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger)
    {
        applicationBuilder.AddNpgsqlDbContext<RecipesDbContext>(connectionName: "ServerDb");
        
        applicationBuilder.Services.AddFeatures();
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        var recipeModuleGroup = routeGroupBuilder.MapGroup("/recipes-module");
        
        recipeModuleGroup.MapFeatures();
    }
}