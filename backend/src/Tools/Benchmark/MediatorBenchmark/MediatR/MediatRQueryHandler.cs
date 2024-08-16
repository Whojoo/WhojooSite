
using MediatR;

namespace Benchmark.MediatorBenchmark.MediatR;

public sealed class MediatRQueryHandler(HandlerAction handlerAction) : IRequestHandler<MediatRQuery, HandlerResult>
{
    private readonly HandlerAction _handlerAction = handlerAction;

    public async Task<HandlerResult> Handle(MediatRQuery request, CancellationToken cancellationToken)
    {
        return await _handlerAction.Handle(request.A, request.B);
    }
}
