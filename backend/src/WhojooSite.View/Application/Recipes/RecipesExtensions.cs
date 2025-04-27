using WhojooSite.Common;

namespace WhojooSite.View.Application.Recipes;

public static class RecipesExtensions
{
    public static IServiceCollection AddRecipesApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSetting<RecipesModuleSettings>(configuration);

        return services;
    }
}