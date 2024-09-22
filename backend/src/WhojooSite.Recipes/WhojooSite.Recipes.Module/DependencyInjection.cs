using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using WhojooSite.Common.Cqrs;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureRecipesModules(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<RecipesDbContext>("recipes-db");

        builder.Services.AddCqrs<IRecipesModuleAssemblyMarker>();

        return builder;
    }

    public static WebApplication MapRecipesModule(this WebApplication app)
    {
        return app;
    }
}