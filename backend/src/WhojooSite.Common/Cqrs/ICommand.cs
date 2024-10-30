using MediatR;

namespace WhojooSite.Common.Cqrs;

public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
{
}