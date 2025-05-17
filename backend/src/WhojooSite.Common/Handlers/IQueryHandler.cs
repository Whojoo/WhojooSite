using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public interface IQuery;

public interface IQuery<TResult>;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
{
    Task<Result<TQueryResult>> HandleAsync(TQuery query, CancellationToken cancellation);
}