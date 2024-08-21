namespace WhojooSite.Common.Cqrs;

public interface IQueryDispatcher
{
    ValueTask<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TQueryResult>;
}