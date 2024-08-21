namespace WhojooSite.Common.Cqrs;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
    ValueTask<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken);
}