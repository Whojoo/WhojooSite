using Microsoft.AspNetCore.Http;

namespace WhojooSite.Common.Api;

public class DomainEventFilter(Queue<int> domainEventStorage) : IEndpointFilter
{
    private readonly Queue<int> _domainEventStorage = domainEventStorage;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        context.HttpContext
            .Response
            .OnCompleted(() =>
            {
                foreach (var @event in _domainEventStorage)
                {
                    Console.WriteLine(@event);
                }
                
                return Task.CompletedTask;
            });

        return await next(context);
    }
}