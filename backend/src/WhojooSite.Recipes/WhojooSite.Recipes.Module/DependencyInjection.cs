using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Web;

namespace WhojooSite.Recipes.Module;

public static class DependencyInjection
{
    public static IServiceCollection AddRecipesModule(this IServiceCollection services)
    {
        var currentAssembly = typeof(IRecipesModuleAssemblyMarker).Assembly;
        return services;
    }

    public static WebApplication MapRecipesModule(this WebApplication app)
    {
        return app;
    }
}
