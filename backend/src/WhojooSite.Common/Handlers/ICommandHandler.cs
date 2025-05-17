using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public interface ICommand;

public interface ICommand<TResult>;

public interface ICommandHandler<in TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
    Task<Result<TCommandResult>> HandleAsync(TCommand command, CancellationToken cancellation);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellation);
}