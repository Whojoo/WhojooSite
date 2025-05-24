using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public interface IRequest<TResponse>;

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}

public interface IQuery<TResult> : IRequest<TResult>;

public interface IQueryHandler<in TQuery, TQueryResult> : IRequestHandler<TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>;

public interface ICommand<TResult> : IRequest<TResult>;

public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>;