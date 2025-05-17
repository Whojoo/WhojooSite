using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Api;

public static class ApiExtensions
{
    public static IServiceCollection AddEndpoints<TModuleEndpointInterface>(this IServiceCollection services)
        where TModuleEndpointInterface : IEndpoint
    {
        services.Scan(scan => scan.FromAssemblyOf<TModuleEndpointInterface>()
            .AddClasses(classes => classes.AssignableTo<TModuleEndpointInterface>(), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints<TModuleEndpointInterface>(this IEndpointRouteBuilder endpointRouteBuilder)
        where TModuleEndpointInterface : IEndpoint
    {
        var endpoints = endpointRouteBuilder.ServiceProvider.GetServices<TModuleEndpointInterface>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}