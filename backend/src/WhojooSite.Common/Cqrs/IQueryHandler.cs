namespace WhojooSite.Common.Cqrs;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IQuery<TQueryResult>
{
    ValueTask<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken);
}