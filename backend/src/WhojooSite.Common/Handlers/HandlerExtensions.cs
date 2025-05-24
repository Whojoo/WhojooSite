using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Handlers;

public static class HandlerExtensions
{
    public static IServiceCollection AddHandlers<TAssemblyMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<TAssemblyMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static IServiceCollection AddDispatchers(this IServiceCollection services)
    {
        services.AddScoped(typeof(IQueryDispatcher<,>), typeof(Dispatchers.QueryDispatcher<,>));
        services.AddScoped(typeof(ICommandDispatcher<,>), typeof(Dispatchers.CommandDispatcher<,>));
        services.AddSingleton(typeof(IRequestPipelineHandler<,>), typeof(LoggingRequestPipelineHandler<,>));
        services.AddSingleton(typeof(IRequestPipelineHandler<,>), typeof(ExceptionRequestPipelineHandler<,>));
        services.AddSingleton(typeof(IRequestPipelineHandler<,>), typeof(ValidationRequestPipelineHandler<,>));
        return services;
    }
}