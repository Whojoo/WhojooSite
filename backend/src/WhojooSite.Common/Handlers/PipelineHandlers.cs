using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public interface IRequestPipelineHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> HandlerAsync(
        TRequest request,
        Func<TRequest, CancellationToken, Task<Result<TResponse>>> next,
        CancellationToken cancellation);
}

public interface IQueryPipelineHandler<TQuery, TResponse> : IRequestPipelineHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>;

public interface ICommandPipelineHandler<TCommand, TResponse> : IRequestPipelineHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;