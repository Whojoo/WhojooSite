using MediatR;

namespace WhojooSite.Common.Cqrs;

public interface ICommandHandler<in TCommand, TCommandResult> : IRequestHandler<TCommand, TCommandResult>
    where TCommand : ICommand<TCommandResult>
{
}