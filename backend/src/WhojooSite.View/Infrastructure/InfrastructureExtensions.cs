using WhojooSite.Common.Config;
using WhojooSite.View.Application.Interfaces;
using WhojooSite.View.Infrastructure.Integrations;
using WhojooSite.View.Infrastructure.Integrations.Fuel;
using WhojooSite.View.SharedKernel;

namespace WhojooSite.View.Infrastructure;

internal static class InfrastructureExtensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfig<FuelModuleConfig>(configuration);

        services.AddKiotaHandlers()
            .AddFuelModuleClient();

        return services;
    }

    private static IServiceCollection AddFuelModuleClient(this IServiceCollection services)
    {
        services.AddHttpClient<FuelModuleClientFactory>((sp, client) =>
        {
            var fuelConfig = sp.GetRequiredService<FuelModuleConfig>();
            client.BaseAddress = fuelConfig.BaseUrl;
        }).AttachKiotaHandlers();

        services.AddTransient(sp => sp.GetRequiredService<FuelModuleClientFactory>().GetClient());

        services.AddTransient<IFuelModule, HttpFuelModuleIntegration>();

        return services;
    }
}