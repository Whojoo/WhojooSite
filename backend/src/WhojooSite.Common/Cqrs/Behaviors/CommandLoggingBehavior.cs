using System.Diagnostics;
using System.Reflection;

using Ardalis.GuardClauses;

using MediatR;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Behaviors;

public class CommandLoggingBehavior<TCommand, TCommandResult>(
    ILogger<CommandLoggingBehavior<TCommand, TCommandResult>> logger,
    IWebHostEnvironment environment)
    : IPipelineBehavior<TCommand, TCommandResult> where TCommand : ICommand<TCommandResult>
{
    private readonly ILogger<CommandLoggingBehavior<TCommand, TCommandResult>> _logger = logger;
    private readonly bool _shouldLogMembers = environment.IsDevelopment();

    public async Task<TCommandResult> Handle(
        TCommand request,
        RequestHandlerDelegate<TCommandResult> next,
        CancellationToken cancellationToken)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return await next();
        }

        Guard.Against.Null(request);

        _logger.LogInformation("Executing command {CommandName}", typeof(TCommand).Name);

        LogRequestMembers(request);

        var startTime = Stopwatch.GetTimestamp();

        var response = await next();

        var elapsed = Stopwatch.GetElapsedTime(startTime);

        _logger.LogInformation(
            "Finished execution of command {CommandName} with {Response} in {ElapsedMilliseconds} ms",
            typeof(TCommand).Name,
            typeof(TCommandResult),
            elapsed.Milliseconds);

        return response;
    }

    private void LogRequestMembers(TCommand request)
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