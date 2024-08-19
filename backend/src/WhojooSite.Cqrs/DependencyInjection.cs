using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using WhojooSite.Cqrs.Impl;

namespace WhojooSite.Cqrs;

public static class DependencyInjection
{
    public static IServiceCollection AddCqrs<TAssemblyMarker>(
        this IServiceCollection services,
        ServiceLifetime handlerLifetime = ServiceLifetime.Scoped)
    {
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();

        services
            .Scan(selector => selector
                .FromAssemblyOf<TAssemblyMarker>()
                .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithLifetime(handlerLifetime));

        services
            .Scan(selector => selector
                .FromAssemblyOf<TAssemblyMarker>()
                .AddClasses(filter => filter.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithLifetime(handlerLifetime));

        return services;
    }
}