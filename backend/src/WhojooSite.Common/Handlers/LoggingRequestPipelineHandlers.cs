using System.Diagnostics;

using Microsoft.Extensions.Logging;

using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public partial class LoggingRequestPipelineHandler<TRequest, TResponse>(ILogger<LoggingRequestPipelineHandler<TRequest, TResponse>> logger)
    : IRequestPipelineHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingRequestPipelineHandler<TRequest, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> HandlerAsync(
        TRequest query,
        Func<TRequest, CancellationToken, Task<Result<TResponse>>> next,
        CancellationToken cancellation)
    {
        Log.HandlingRequest(_logger, typeof(TRequest).Name);
        var startingTimestamp = Stopwatch.GetTimestamp();

        var result = await next(query, cancellation);

        var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

        result.Tap(
            _ => Log.HandledSuccessfully(_logger, typeof(TRequest).Name, elapsed.TotalMilliseconds),
            errors => Log.HandleFailed(_logger, typeof(TRequest).Name, errors[0].Status.ToString(), elapsed.TotalMilliseconds));

        return result;
    }

    private static partial class Log
    {
        [LoggerMessage(
            LogLevel.Information,
            Message = "handling request {RequestName}")]
        public static partial void HandlingRequest(ILogger logger, string requestName);

        [LoggerMessage(
            LogLevel.Information,
            Message = "Request {RequestName} handled successfully in {ElapsedMilliseconds} ms")]
        public static partial void HandledSuccessfully(ILogger logger, string requestName, double elapsedMilliseconds);

        [LoggerMessage(
            LogLevel.Warning,
            Message = "Request {RequestName} failed with status {RequestFailureStatus} in {ElapsedMilliseconds} ms")]
        public static partial void HandleFailed(ILogger logger, string requestName, string requestFailureStatus,
            double elapsedMilliseconds);
    }
}