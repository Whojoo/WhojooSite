using System.Diagnostics;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Impl;

internal sealed class QueryDispatcher(
    IServiceProvider serviceProvider,
    ILogger<QueryDispatcher> logger,
    IWebHostEnvironment environment)
    : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<QueryDispatcher> _logger = logger;
    private readonly bool _shouldLogMembers = environment.IsDevelopment();

    public async ValueTask<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query,
        CancellationToken cancellationToken = default)
        where TQuery : IQuery<TQueryResult>
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TQueryResult>>();

        _logger.LogInformation("Executing query {Query}", typeof(TQuery).Name);

        LogRequestMembers(query);

        var stopwatch = Stopwatch.StartNew();
        var result = await handler.Handle(query, cancellationToken);
        var elapsed = stopwatch.Elapsed;

        _logger.LogInformation(
            "Finished execution of query {QueryName} with {Response} in {ElapsedMilliseconds} ms",
            typeof(TQuery).Name,
            typeof(TQueryResult),
            elapsed.TotalMilliseconds);
        return result;
    }

    private void LogRequestMembers<TQuery>(TQuery query) where TQuery : notnull
    {
        // This method uses reflection which could affect performance, so only use this for development.
        if (!_shouldLogMembers)
        {
            return;
        }

        Type myType = query.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            object? propValue = prop?.GetValue(query, null);
            _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
        }
    }
}