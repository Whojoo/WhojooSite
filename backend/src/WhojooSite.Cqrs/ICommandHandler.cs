namespace WhojooSite.Cqrs;

public interface ICommandHandler<TCommand, TCommandResult>
{
    Task<TCommandResult> Handle(TCommand command, CancellationToken cancellationToken);
}