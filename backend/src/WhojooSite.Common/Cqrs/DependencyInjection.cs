using System.Text;

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

public class Solution {
    public string IntToRoman(int num)
    {
        char[] characters = ['M', 'D', 'C', 'L', 'X', 'V', 'I'];
        int[] values = [1000, 500, 100, 50, 10, 5, 1];

        var stringBuilder = new StringBuilder();

        for (var i = 0; i < characters.Length; i++)
        {
            var input = string.Empty;
            var maxRotations = i % 2 == 0 ? 1 : 4;
            var rotations = 0;

            while (num <= values[i] && rotations < maxRotations)
            {
                num -= values[i];
                input += characters[i];
                rotations++;
            }

            if (input.Length > 3)
            {
                input = $"{characters[i]}{characters[i - 1]}";
            }

            if (input.Length > 0)
            {
                stringBuilder.Append(input);
            }
        }

        return stringBuilder.ToString();
    }
}