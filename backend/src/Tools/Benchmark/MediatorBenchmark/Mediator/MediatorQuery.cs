using Mediator;

namespace Benchmark.MediatorBenchmark.Mediator;

public record MediatorQuery(int A, int B) : IRequest<HandlerResult>;