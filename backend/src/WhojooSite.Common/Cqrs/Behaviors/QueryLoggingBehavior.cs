using System.Diagnostics;
using System.Reflection;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Behaviors;

public class QueryLoggingBehavior<TQuery, TQueryResult>(
    ILogger<QueryLoggingBehavior<TQuery, TQueryResult>> logger,
    IWebHostEnvironment environment)
    : IPipelineBehavior<TQuery, TQueryResult> where TQuery : IQuery<TQueryResult>
{
    private readonly ILogger<QueryLoggingBehavior<TQuery, TQueryResult>> _logger = logger;
    private readonly bool _shouldLogMembers = environment.IsDevelopment();

    public async Task<TQueryResult> Handle(
        TQuery request,
        RequestHandlerDelegate<TQueryResult> next,
        CancellationToken cancellationToken)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return await next();
        }

        _logger.LogInformation("Executing query {QueryName}", typeof(TQuery).Name);

        LogRequestMembers(request);

        var startTime = Stopwatch.GetTimestamp();

        var response = await next();

        var elapsed = Stopwatch.GetElapsedTime(startTime);

        _logger.LogInformation(
            "Finished execution of query {QueryName} with {Response} in {ElapsedMilliseconds} ms",
            typeof(TQuery).Name,
            typeof(TQueryResult),
            elapsed.Milliseconds);

        return response;
    }

    private void LogRequestMembers(TQuery request)
    {
        // This method uses reflection which could affect performance, so only use this for development.
        if (!_shouldLogMembers)
        {
            return;
        }

        Type myType = request.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            object? propValue = prop?.GetValue(request, null);
            _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
        }
    }
}