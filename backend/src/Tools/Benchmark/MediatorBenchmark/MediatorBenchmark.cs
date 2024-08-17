using System.Runtime.InteropServices;

using Benchmark.MediatorBenchmark.MediatR;
using Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

using BenchmarkDotNet.Attributes;

using MediatR;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Cqrs;

namespace Benchmark.MediatorBenchmark;

[MemoryDiagnoser]
public class MediatorBenchmark
{
    private IServiceProvider _mediatRProvider = null!;
    private IServiceProvider _whojooSiteCqrsProvider = null!;
    private IServiceProvider _baselineProvider = null!;

    [Params(1, 1000, 1000000)]
    public int Iterations;

    private List<HandlerResult> _list = new List<HandlerResult>();

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
        _list = new List<HandlerResult>(Iterations);
    }

    [Benchmark(Baseline = true)]
    public async Task<List<HandlerResult>> Baseline()
    {
        for (var i = 0; i < Iterations; i++)
        {
            using (var scope = _baselineProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<HandlerAction>();
                var result = await handler.Handle(i, 2);
                _list.Add(result);
            }
        }
        return _list;
    }

    [Benchmark]
    public async Task<List<HandlerResult>> MediatR()
    {
        for (var i = 0; i < Iterations; i++)
        {
            using (var scope = _mediatRProvider.CreateScope())
            {
                var query = new MediatRQuery(i, 2);
                var mediatR = scope.ServiceProvider.GetRequiredService<IMediator>();
                var result = await mediatR.Send(query);
                _list.Add(result);
            }
        }

        return _list;
    }

    [Benchmark]
    public async Task<List<HandlerResult>> WhojooSiteCqrs()
    {
        for (var i = 0; i < Iterations; i++)
        {
            using (var scope = _whojooSiteCqrsProvider.CreateScope())
            {
                var query = new WhojooSiteQuery(i, 2);
                var dispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
                var result = await dispatcher.Dispatch<WhojooSiteQuery, HandlerResult>(query);
                _list.Add(result);
            }
        }

        return _list;
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

    private void SetupBaseline()
    {
        var baselineCollection = new ServiceCollection();
        baselineCollection.AddScoped<HandlerAction>();
        _baselineProvider = baselineCollection.BuildServiceProvider();
    }
}