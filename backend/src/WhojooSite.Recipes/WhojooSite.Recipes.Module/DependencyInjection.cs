using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Cqrs;
using WhojooSite.Common.Web;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module;

public static class DependencyInjection
{
    public static IServiceCollection AddRecipesModule(this IServiceCollection services)
    {
        services.AddCqrs<IRecipesModuleAssemblyMarker>();
        // services.AddEndpoints<IRecipesModuleAssemblyMarker>();
        // services.AddDbContext<RecipesDbContext>(options =>
        // {
        //     options
        //         .UseNpgsql()
        // })

        return services;
    }

    public static WebApplication MapRecipesModule(this WebApplication app)
    {
        return app;
    }
}