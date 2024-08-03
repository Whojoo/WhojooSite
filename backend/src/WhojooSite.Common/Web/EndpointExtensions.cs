using System.Reflection;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WhojooSite.Common.Web;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
        => services.AddEndpoints([Assembly.GetEntryAssembly()!]);

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly[] assemblies)
    {
        var endpointType = typeof(IEndpoint);
        var fullInterfaceName = endpointType.FullName!;

        var endpointTypes = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .Where(type => type.GetInterface(fullInterfaceName) is not null);

        foreach (var type in endpointTypes)
        {
            services.TryAddScoped(endpointType, type);
        }

        return services;
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        using var scope = app.ServiceProvider.CreateScope();
        var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.AddEndpoint(app);
        }

        return app;
    }
}