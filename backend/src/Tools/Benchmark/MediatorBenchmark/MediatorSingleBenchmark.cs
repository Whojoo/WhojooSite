using Benchmark.MediatorBenchmark.Mediator;
using Benchmark.MediatorBenchmark.MediatR;
using Benchmark.MediatorBenchmark.WhojooSite.Cqrs;

using BenchmarkDotNet.Attributes;

using ExternalMediator = Mediator;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Cqrs;

namespace Benchmark.MediatorBenchmark;

[MemoryDiagnoser]
public class MediatorSingleBenchmark
{
    private IServiceProvider _serviceProvider = null!;

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

    [Benchmark(Baseline = true)]
    public async Task<HandlerResult?> Baseline()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<HandlerAction>();
        return await handler.Handle(2, 4);
    }

    [Benchmark]
    public async Task<HandlerResult?> MediatR()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var query = new MediatRQuery(2, 4);
        var mediatR = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediatR.Send(query);
    }

    [Benchmark]
    public async Task<HandlerResult?> WhojooSiteCqrs()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var query = new WhojooSiteQuery(2, 4);
        var queryDispatcher = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
        return await queryDispatcher.Dispatch<WhojooSiteQuery, HandlerResult>(query);
    }

    [Benchmark]
    public async Task<HandlerResult?> Mediator()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var query = new MediatorQuery(2, 4);
        var mediator = scope.ServiceProvider.GetRequiredService<ExternalMediator.IMediator>();
        return await mediator.Send(query);
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