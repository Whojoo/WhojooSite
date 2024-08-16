using MediatR;

namespace Benchmark.MediatorBenchmark.MediatR;

public record MediatRQuery(int A, int B) : IRequest<HandlerResult>;