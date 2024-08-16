namespace Benchmark.MediatorBenchmark;

public sealed class HandlerAction
{
    public Task<HandlerResult> Handle(int a, int b) => Task.FromResult(new HandlerResult(a * b));
}