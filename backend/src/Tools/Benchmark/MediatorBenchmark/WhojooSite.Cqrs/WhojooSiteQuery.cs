using WhojooSite.Cqrs;

namespace Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

public record WhojooSiteQuery(int A, int B) : IQuery<HandlerResult>;