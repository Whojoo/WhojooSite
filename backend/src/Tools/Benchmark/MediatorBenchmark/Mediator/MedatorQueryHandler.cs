
using Mediator;

namespace Benchmark.MediatorBenchmark.Mediator;

public sealed class MediatorQueryHandler(HandlerAction handlerAction) : IRequestHandler<MediatorQuery, HandlerResult>
{
    private readonly HandlerAction _handlerAction = handlerAction;

    public async ValueTask<HandlerResult> Handle(MediatorQuery request, CancellationToken cancellationToken)
    {
        return await _handlerAction.Handle(request.A, request.B);
    }
}
