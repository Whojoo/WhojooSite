using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module;

public class RecipesModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Recipes";

    public void ConfigureModule(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
        services.AddDbContext<RecipesDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString(DataSchemaConstants.ConnectionStringName);
            options.UseNpgsql(connectionString);
        });
    }

    public void MapModule(WebApplication app, ILogger logger)
    {
    }

    public bool HasEndpoints() => true;
}