using Benchmark.MediatorBenchmark.MediatR;
using Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

using BenchmarkDotNet.Attributes;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Cqrs;

namespace Benchmark.MediatorBenchmark;

[MemoryDiagnoser]
public class MediatorSingleBenchmark
{
    private IServiceProvider _mediatRProvider = null!;
    private IServiceProvider _whojooSiteCqrsProvider = null!;
    private IServiceProvider _baselineProvider = null!;

    private IServiceScope _mediatRScope = null!;
    private IServiceScope _whojooSiteCqrsScope = null!;
    private IServiceScope _baselineScope = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        SetupMediatR();
        SetupWhojooSiteCqrs();
        SetupBaseline();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _mediatRScope = _mediatRProvider.CreateScope();
        _whojooSiteCqrsScope = _whojooSiteCqrsProvider.CreateScope();
        _baselineScope = _baselineProvider.CreateScope();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _mediatRScope.Dispose();
        _whojooSiteCqrsScope.Dispose();
        _baselineScope.Dispose();
    }

    [Benchmark(Baseline = true)]
    public async Task<HandlerResult?> Baseline()
    {
        var handler = _baselineProvider.GetRequiredService<HandlerAction>();
        return await handler.Handle(2, 4);
    }

    [Benchmark]
    public async Task<HandlerResult?> MediatR()
    {
        var query = new MediatRQuery(2, 4);
        var mediatR = _mediatRScope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediatR.Send(query);
    }

    [Benchmark]
    public async Task<HandlerResult?> WhojooSiteCqrs()
    {
        var query = new WhojooSiteQuery(2, 4);
        var queryDispatcher = _whojooSiteCqrsScope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        return await queryDispatcher.Dispatch<WhojooSiteQuery, HandlerResult>(query);
    }

    private void SetupBaseline()
    {
        var baseLineServiceCollection = new ServiceCollection();
        baseLineServiceCollection.AddScoped<HandlerAction>();
        _baselineProvider = baseLineServiceCollection.BuildServiceProvider();
    }

    private void SetupMediatR()
    {
        var mediatRServiceCollection = new ServiceCollection();
        mediatRServiceCollection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<MediatorBenchmark>();
        });
        mediatRServiceCollection.AddScoped<HandlerAction>();
        _mediatRProvider = mediatRServiceCollection.BuildServiceProvider();
    }

    private void SetupWhojooSiteCqrs()
    {
        var whojooSiteCqrsCollection = new ServiceCollection();
        whojooSiteCqrsCollection.AddCqrs<MediatorBenchmark>();
        whojooSiteCqrsCollection.AddScoped<HandlerAction>();
        _whojooSiteCqrsProvider = whojooSiteCqrsCollection.BuildServiceProvider();
    }
}