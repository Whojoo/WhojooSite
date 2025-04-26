using WhojooSite.Common;

namespace WhojooSite.Recipes.Module.Application;

internal abstract class QueryTemplateMethod<THandler, TQuery, TResponse>
    where TQuery : ITemplateQuery<TResponse>
    where THandler : QueryTemplateMethod<THandler, TQuery, TResponse>
{
    public Task<Result<TResponse>> HandleAsync(TQuery query)
    {
        return HandleQueryAsync(query);
    }

    protected abstract Task<Result<TResponse>> HandleQueryAsync(TQuery query);
}

internal interface ITemplateQuery<out TQueryResponse>;