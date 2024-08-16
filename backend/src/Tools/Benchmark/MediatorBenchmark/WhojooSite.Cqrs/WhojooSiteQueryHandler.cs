
using WhojooSite.Cqrs;

namespace Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

public sealed class WhojooSiteQueryHandler(HandlerAction handlerAction) : IQueryHandler<WhojooSiteQuery, HandlerResult>
{
    private readonly HandlerAction _handlerAction = handlerAction;

    public async Task<HandlerResult> Handle(WhojooSiteQuery query, CancellationToken cancellationToken)
    {
        return await _handlerAction.Handle(query.A, query.B);
    }
}
