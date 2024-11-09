using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WhojooSite.Common.Persistence;

public static class PersistenceExtensions
{
    public static IServiceCollection AddNpgsqlConnectionFactory(this IServiceCollection services)
    {
        services.TryAddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>();

        return services;
    }
}