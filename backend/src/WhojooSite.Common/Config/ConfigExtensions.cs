using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Config;

public static class ConfigExtensions
{
    public static IServiceCollection AddConfig<TConfig>(this IServiceCollection services, IConfiguration configuration)
        where TConfig : class, IConfig
    {
        services.AddOptions<TConfig>()
            .Configure(config => configuration.Bind(config.Position, config));

        return services;
    }
}