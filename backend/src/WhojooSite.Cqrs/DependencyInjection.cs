using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using WhojooSite.Cqrs.Impl;

namespace WhojooSite.Cqrs;

public static class DependencyInjection
{
    public static IServiceCollection AddCqrs<TAssemblyMarker>(this IServiceCollection services)
    {
        services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.Scan(selector => selector
            .FromAssemblyOf<TAssemblyMarker>()
            .AddClasses(filter => filter.AssignableTo(typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(selector => selector
            .FromAssemblyOf<TAssemblyMarker>()
            .AddClasses(filter => filter.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}