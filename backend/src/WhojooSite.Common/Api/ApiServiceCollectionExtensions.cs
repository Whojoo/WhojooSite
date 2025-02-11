using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Api;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddEndpointsForAssembly(this IServiceCollection services, Assembly assembly)
    {
        var endpoints = assembly
            .GetTypes()
            .Where(type => typeof(IEndpoint).IsAssignableFrom(type) &&
                           type != typeof(IEndpoint) &&
                           !type.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            services.AddTransient(typeof(IEndpoint), endpoint);
        }
        
        return services;
    }
}