using MediatR;

namespace WhojooSite.Common.Cqrs;

public interface IQuery<out TQueryResult> : IRequest<TQueryResult>
{
}