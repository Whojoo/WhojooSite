using Dapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WhojooSite.Common.Cqrs;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Domain.SpiceMix;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureRecipesModules(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<RecipesDbContext>(DataSchemaConstants.DbName);

        builder.Services.AddScoped<RecipesDbConnectionFactory>();
        builder.Services.AddCqrs<IRecipesModuleAssemblyMarker>();

        RegisterTypeHandlers();

        return builder;
    }

    public static WebApplication MapRecipesModule(this WebApplication app)
    {
        return app;
    }

    private static void RegisterTypeHandlers()
    {
        SqlMapper.AddTypeHandler(new OwnerId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new CookbookId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new RecipeId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new StepId.DapperTypeHandler());
        SqlMapper.AddTypeHandler(new SpiceMixId.DapperTypeHandler());
    }
}