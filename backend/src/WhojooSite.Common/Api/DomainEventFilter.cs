using Microsoft.AspNetCore.Http;

namespace WhojooSite.Common.Api;

public class DomainEventFilter(Queue<int> domainEventStorage) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        context.HttpContext
            .Response
            .OnCompleted(() =>
            {
                foreach (var @event in domainEventStorage)
                {
                    Console.WriteLine(@event);
                }

                return Task.CompletedTask;
            });

        return await next(context);
    }
}