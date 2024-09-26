using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Impl;

internal sealed class QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger)
    : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<QueryDispatcher> _logger = logger;

    public ValueTask<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TQueryResult>
    {
        _logger.LogInformation("Executing query {QueryName}", typeof(TQuery).Name);
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();
        _logger.LogInformation("Finished execution of query {QueryName}", typeof(TQuery).Name);
        return handler.Handle(query, cancellationToken);
    }
}