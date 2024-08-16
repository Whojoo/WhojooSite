namespace WhojooSite.Cqrs;

public interface IQueryHandler<TQuery, TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellationToken);
}