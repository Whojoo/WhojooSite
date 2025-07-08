using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common;
using WhojooSite.Common.Handlers;
using WhojooSite.Common.Modules;
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

        applicationBuilder.Services
            .AddHandlers<IRecipeModuleAssemblyMarker>()
            .AddValidators<IRecipeModuleAssemblyMarker>();
    }
}