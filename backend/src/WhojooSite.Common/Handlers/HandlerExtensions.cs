using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Handlers;

public static class HandlerExtensions
{
    public static IServiceCollection AddHandlers<TAssemblyMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<TAssemblyMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}