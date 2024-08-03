using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Web;

namespace WhojooSite.Recipes.Module;

public static class DependencyInjection
{
    public static IServiceCollection AddRecipesModule(this IServiceCollection services)
    {
        return services;
    }

    public static IApplicationBuilder MapRecipesModule(this IApplicationBuilder app)
    {
        return app;
    }
}
