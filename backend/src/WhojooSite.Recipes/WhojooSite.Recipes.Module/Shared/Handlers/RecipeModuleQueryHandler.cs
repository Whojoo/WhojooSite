using System.Diagnostics;

using FluentValidation;

using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Shared.Handlers;

internal abstract class RecipeModuleQueryHandler<TQuery, TResponse>(
    ILogger<RecipeModuleQueryHandler<TQuery, TResponse>> logger,
    IValidator<TQuery>? validator = null)
    : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    private readonly ILogger<RecipeModuleQueryHandler<TQuery, TResponse>> _logger = logger;
    private readonly IValidator<TQuery>? _validator = validator;

    public async Task<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellation)
    {
        _logger.LogInformation("Handling query {QueryName}", typeof(TQuery).Name);
        var startingTimestamp = Stopwatch.GetTimestamp();

        if (_validator is not null)
        {
            // These validations should not do any IO stuff, so use the sync method without cancellation
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation
            var validationResult = _validator.Validate(query);
            if (!validationResult.IsValid)
            {
                var resultErrors = validationResult
                    .Errors
                    .Select(error => new ResultError(error.PropertyName, error.ErrorMessage, ResultStatus.BadRequest))
                    .ToList();

                var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

                _logger.LogWarning(
                    "Query {QueryHandler} failed with status {QueryFailureStatus} in {ElapsedMilliseconds} ms",
                    typeof(TQuery).Name,
                    "InputValidation",
                    elapsed.TotalMilliseconds);

                return Result<TResponse>.Failure(resultErrors);
            }
        }

        Result<TResponse> result;

        try
        {
            result = await HandleQueryAsync(query, cancellation);

            var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

            result.Tap(
                _ => _logger.LogInformation(
                    "Query {QueryHandler} handled successfully in {ElapsedMilliseconds} ms",
                    typeof(TQuery).Name,
                    elapsed.TotalMilliseconds),
                errors => _logger.LogWarning(
                    "Query {QueryHandler} failed with status {QueryFailureStatus} in {elapsedMilliseconds} ms",
                    typeof(TQuery).Name,
                    errors[0].Status,
                    elapsed.TotalMilliseconds));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling query {QueryName}", typeof(TQuery).Name);
            result = Result<TResponse>.Failure();
        }

        return result;
    }

    protected abstract Task<Result<TResponse>> HandleQueryAsync(TQuery query, CancellationToken cancellation);
}