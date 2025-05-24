using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public interface IQueryDispatcher<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> DispatchAsync(TQuery query, CancellationToken cancellationToken);
}

public interface ICommandDispatcher<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> DispatchAsync(TCommand command, CancellationToken cancellationToken);
}

internal static class Dispatchers
{
    public class QueryDispatcher<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> queryHandler,
        IEnumerable<IRequestPipelineHandler<TQuery, TResponse>> pipelineHandlers)
        : IQueryDispatcher<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        private readonly IEnumerable<IRequestPipelineHandler<TQuery, TResponse>> _pipelineHandlers = pipelineHandlers;
        private readonly IQueryHandler<TQuery, TResponse> _queryHandler = queryHandler;

        public Task<Result<TResponse>> DispatchAsync(TQuery query, CancellationToken cancellationToken)
        {
            return _pipelineHandlers
                .Reverse()
                .Aggregate<IRequestPipelineHandler<TQuery, TResponse>, Func<TQuery, CancellationToken, Task<Result<TResponse>>>>(
                    (request, cancellation) => _queryHandler.HandleAsync(request, cancellation),
                    (next, pipeline) => (request, cancellation) => pipeline.HandlerAsync(request, next, cancellation))
                (query, cancellationToken);
        }
    }

    public class CommandDispatcher<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> commandHandler,
        IEnumerable<IRequestPipelineHandler<TCommand, TResponse>> pipelineHandlers)
        : ICommandDispatcher<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _commandHandler = commandHandler;
        private readonly IEnumerable<IRequestPipelineHandler<TCommand, TResponse>> _pipelineHandlers = pipelineHandlers;

        public Task<Result<TResponse>> DispatchAsync(TCommand command, CancellationToken cancellationToken)
        {
            return _pipelineHandlers
                .Reverse()
                .Aggregate<IRequestPipelineHandler<TCommand, TResponse>, Func<TCommand, CancellationToken, Task<Result<TResponse>>>>(
                    (request, cancellation) => _commandHandler.HandleAsync(request, cancellation),
                    (next, pipeline) => (request, cancellation) => pipeline.HandlerAsync(request, next, cancellation))
                (command, cancellationToken);
        }
    }
}