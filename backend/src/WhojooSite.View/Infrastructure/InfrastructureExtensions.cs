using WhojooSite.Common;
using WhojooSite.Common.Protos;
using WhojooSite.View.Infrastructure.Recipes;

namespace WhojooSite.View.Infrastructure;

internal static class InfrastructureExtensions
{
    internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddRecipesInfrastructure();

        return services;
    }


    internal static Result<T> MapToFailureResult<T>(this OperationFailureResult operationFailureResult)
    {
        return Result.Failure<T>(operationFailureResult
            .Errors
            .Select(error => new ResultError(error.Code, error.Descriptions.Select(desc => desc.Description))));
    }
}