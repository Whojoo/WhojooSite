using Benchmark.MediatorBenchmark.MediatR;
using Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

using BenchmarkDotNet.Attributes;
using ExternalMediator = Mediator;
using MediatR;

using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Cqrs;
using Benchmark.MediatorBenchmark.Mediator;

namespace Benchmark.MediatorBenchmark;

[MemoryDiagnoser]
public class MediatorBenchmark
{
    private IServiceProvider _serviceProvider = null!;

    [Params(1_000_000)]
    public int Iterations;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<HandlerAction>();
        SetupMediatR(serviceCollection);
        SetupWhojooSiteCqrs(serviceCollection);
        SetupMediator(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Benchmark]
    public async Task<HandlerResult?> Baseline()
    {
        HandlerResult? result = null;
        for (var i = 0; i < Iterations; i++)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var handler = scope.ServiceProvider.GetRequiredService<HandlerAction>();
            result = await handler.Handle(i, 2);
        }

        return result;
    }

    [Benchmark]
    public async Task<HandlerResult?> MediatR()
    {
        HandlerResult? result = null;
        for (var i = 0; i < Iterations; i++)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var query = new MediatRQuery(i, 2);
            var mediatR = scope.ServiceProvider.GetRequiredService<IMediator>();
            result = await mediatR.Send(query);
        }

        return result;
    }

    [Benchmark]
    public async Task<HandlerResult?> WhojooSiteCqrs()
    {
        HandlerResult? result = null;
        for (var i = 0; i < Iterations; i++)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var query = new WhojooSiteQuery(i, 2);
            var dispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
            result = await dispatcher.Dispatch<WhojooSiteQuery, HandlerResult>(query);
        }

        return result;
    }

    [Benchmark]
    public async Task<HandlerResult?> Mediator()
    {
        HandlerResult? result = null;
        for (var i = 0; i < Iterations; i++)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var query = new MediatorQuery(i, 2);
            var mediator = scope.ServiceProvider.GetRequiredService<ExternalMediator.IMediator>();
            result = await mediator.Send(query);
        }

        return result;
    }

    private static void SetupMediatR(ServiceCollection serviceCollection)
    {
        serviceCollection.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining<MediatorBenchmark>();
        });
    }

    private static void SetupWhojooSiteCqrs(ServiceCollection serviceCollection)
    {
        serviceCollection.AddCqrs<MediatorBenchmark>();
    }

    private static void SetupMediator(ServiceCollection serviceCollection)
    {
        serviceCollection.AddMediator();
    }
}