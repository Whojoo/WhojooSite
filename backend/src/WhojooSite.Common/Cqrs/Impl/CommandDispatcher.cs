using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Cqrs.Impl;

internal sealed class CommandDispatcher(
    IServiceProvider serviceProvider,
    ILogger<CommandDispatcher> logger) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<CommandDispatcher> _logger = logger;

    public async ValueTask<TCommandResult> Dispatch<TCommand, TCommandResult>(
        TCommand command, 
        CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>
    {
        _logger.LogInformation("Executing command {CommandName}", typeof(TCommand).Name);
        
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        
        var result = await handler.Handle(command, cancellationToken);
        
        _logger.LogInformation("Finished execution of command {CommandName}", typeof(TCommand).Name);

        return result;
    }
}
