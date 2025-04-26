using System.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Api;

internal class LoggingEndpointFilter<T>(ILogger<LoggingEndpointFilter<T>> logger) : IEndpointFilter
{
    private readonly string _requestName = typeof(T).Name;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        logger.LogInformation("Starting request {RequestName}", _requestName);
        var startTime = Stopwatch.GetTimestamp();

        var result = await next(context);

        logger.LogInformation(
            "Finished request {RequestName} in {ElapsedMilliseconds}ms",
            _requestName,
            Stopwatch.GetElapsedTime(startTime).TotalMilliseconds);

        return result;
    }
}