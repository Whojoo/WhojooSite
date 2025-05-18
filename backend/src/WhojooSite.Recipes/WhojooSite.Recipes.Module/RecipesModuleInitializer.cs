using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common;
using WhojooSite.Common.Api;
using WhojooSite.Common.Handlers;
using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module.Features;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.Module;

public class RecipesModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Recipes";

    public void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger)
    {
        applicationBuilder.Services.AddDbContext<RecipesDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseNpgsql(
                applicationBuilder.Configuration.GetConnectionString("ServerDb"),
                options => options.MigrationsHistoryTable("__EFMigrationsHistory", "recipes"));
        });

        // applicationBuilder.EnrichNpgsqlDbContext<RecipesDbContext>();

        applicationBuilder.Services
            .AddEndpoints<IRecipeModuleEndpoint>()
            .AddHandlers<IRecipeModuleAssemblyMarker>()
            .AddValidators<IRecipeModuleAssemblyMarker>();
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        var recipeModuleGroup = routeGroupBuilder
            .MapGroup("/recipes-module")
            .WithGroupName("recipes-module");

        recipeModuleGroup.MapEndpoints<IRecipeModuleEndpoint>();
    }

    public void MapModule(WebApplication app)
    {
    }
}