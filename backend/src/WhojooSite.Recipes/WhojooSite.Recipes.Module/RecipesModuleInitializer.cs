using Dapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Common.Persistence;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Domain.SpiceMix;
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

        services.AddNpgsqlConnectionFactory();

        services.AddScoped<RecipesDbConnectionFactory>();

        RegisterTypeHandlers();
    }

    public void MapModule(WebApplication app, ILogger logger)
    {
    }

    public bool HasEndpoints() => true;

    private static void RegisterTypeHandlers()
    {
        SqlMapper.AddTypeHandler(new OwnerId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new CookbookId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new RecipeId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new StepId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new SpiceMixId.DapperTypeHandler());
    }
}