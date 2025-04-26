using FluentValidation;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Api;

public static class ApiEndpointFilterExtensions
{
    public static RouteHandlerBuilder AddRequestLogging<TRequestType>(this RouteHandlerBuilder routeHandlerBuilder)
    {
        return routeHandlerBuilder.AddEndpointFilterFactory((filterFactoryContext, next) =>
        {
            var logger = filterFactoryContext.ApplicationServices
                .GetRequiredService<ILogger<LoggingEndpointFilter<TRequestType>>>();

            if (!logger.IsEnabled(LogLevel.Information))
            {
                return next;
            }

            var loggingFilter = new LoggingEndpointFilter<TRequestType>(logger);

            return invocationContext => loggingFilter.InvokeAsync(invocationContext, next);
        });
    }

    public static RouteHandlerBuilder AddValidation<TValidationType>(this RouteHandlerBuilder routeHandlerBuilder)
    {
        return routeHandlerBuilder.AddEndpointFilterFactory((filterFactoryContext, next) =>
        {
            var validator =
                filterFactoryContext.ApplicationServices.GetService<IValidator<TValidationType>>();

            if (validator is null)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(TValidationType),
                    $"Could not find a validator for object {typeof(TValidationType)}");
            }

            var argumentIndex = -1;
            var parameterInfoEnumerable = filterFactoryContext.MethodInfo
                .GetParameters()
                .Select((item, index) => (index, item));

            foreach (var (i, parameter) in parameterInfoEnumerable)
            {
                if (parameter.ParameterType != typeof(TValidationType))
                {
                    continue;
                }

                argumentIndex = i;
                break;
            }

            if (argumentIndex == -1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(TValidationType),
                    $"Could not find object of type {typeof(TValidationType)} to validate");
            }

            var validationFilter = new ValidationFilter<TValidationType>(validator, argumentIndex);
            return invocationContext => validationFilter.InvokeAsync(invocationContext, next);
        });
    }
}