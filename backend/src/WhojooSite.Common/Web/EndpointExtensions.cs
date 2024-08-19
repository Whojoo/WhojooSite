using System.Reflection;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WhojooSite.Common.Web;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints<TAssemblyMarker>(this IServiceCollection services)
    {
        services
            .Scan(selector => selector
                .FromAssemblyOf<TAssemblyMarker>()
                .AddClasses(filter => filter.AssignableTo<IEndpoint>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

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