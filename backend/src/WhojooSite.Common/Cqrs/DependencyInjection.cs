using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using WhojooSite.Common.Cqrs.Impl;

namespace WhojooSite.Common.Cqrs;

public static class DependencyInjection
{
    public static IServiceCollection AddCqrs<TAssemblyMarker>(
        this IServiceCollection services,
        ServiceLifetime handlerLifetime = ServiceLifetime.Transient)
    {
        services.TryAdd(new ServiceDescriptor(typeof(ICommandDispatcher), typeof(CommandDispatcher), handlerLifetime));
        services.TryAdd(new ServiceDescriptor(typeof(IQueryDispatcher), typeof(QueryDispatcher), handlerLifetime));

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