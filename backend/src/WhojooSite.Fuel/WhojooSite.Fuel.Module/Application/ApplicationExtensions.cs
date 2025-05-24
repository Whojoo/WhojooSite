using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Handlers;

namespace WhojooSite.Fuel.Module.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddHandlers<IFuelModuleAssemblyMarker>();
    }
}