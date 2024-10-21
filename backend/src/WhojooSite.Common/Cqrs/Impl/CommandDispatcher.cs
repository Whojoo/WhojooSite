using System.Diagnostics;
using System.Reflection;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Impl;

internal sealed class CommandDispatcher(
    IServiceProvider serviceProvider,
    ILogger<CommandDispatcher> logger,
    IWebHostEnvironment environment) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<CommandDispatcher> _logger = logger;
    private readonly bool _shouldLogMembers = environment.IsDevelopment();

    public async ValueTask<TCommandResult> Dispatch<TCommand, TCommandResult>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();

        _logger.LogInformation("Executing command {CommandName}", typeof(TCommand).Name);

        LogRequestMembers(command);

        var stopwatch = Stopwatch.StartNew();
        var result = await handler.Handle(command, cancellationToken);
        var elapsed = stopwatch.Elapsed;

        _logger.LogInformation(
            "Finished execution of command {CommandName} in {ElapsedMilliseconds} ms",
            typeof(TCommand).Name,
            elapsed.TotalMilliseconds);

        return result;
    }

    private void LogRequestMembers<TCommand>(TCommand command) where TCommand : notnull
    {
        // This method uses reflection which could affect performance, so only use this for development.
        if (!_shouldLogMembers)
        {
            return;
        }

        Type myType = command.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
        foreach (PropertyInfo prop in props)
        {
            object? propValue = prop?.GetValue(command, null);
            _logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
        }
    }
}