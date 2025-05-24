using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Api;

namespace WhojooSite.Fuel.Module.Endpoints;

internal static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        return services.AddEndpoints<IFuelModuleEndpoint>();
    }

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapEndpoints<IFuelModuleEndpoint>();
    }
}