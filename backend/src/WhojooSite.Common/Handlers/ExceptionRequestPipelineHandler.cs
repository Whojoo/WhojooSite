using Microsoft.Extensions.Logging;

using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public class ExceptionRequestPipelineHandler<TRequest, TResponse>(ILogger<ExceptionRequestPipelineHandler<TRequest, TResponse>> logger)
    : IRequestPipelineHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionRequestPipelineHandler<TRequest, TResponse>> _logger = logger;

    public async Task<Result<TResponse>> HandlerAsync(
        TRequest request,
        Func<TRequest, CancellationToken, Task<Result<TResponse>>> next,
        CancellationToken cancellation)
    {
        try
        {
            return await next(request, cancellation);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception thrown in request");
            return Result<TResponse>.Failure(e.Message);
        }
    }
}