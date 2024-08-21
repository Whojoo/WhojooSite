namespace WhojooSite.Common.Cqrs;

public interface ICommandDispatcher
{
    ValueTask<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TCommandResult>;
}