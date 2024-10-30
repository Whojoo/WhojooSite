using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common.Cqrs.Behaviors;

public static class BehaviorExtensions
{
    public static IServiceCollection AddMediatRLoggingBehaviors(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryLoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandLoggingBehavior<,>));
        return services;
    }
}