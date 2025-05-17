using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace WhojooSite.Common;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddValidators<TAssemblyMarker>(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyOf<TAssemblyMarker>()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)), false)
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}