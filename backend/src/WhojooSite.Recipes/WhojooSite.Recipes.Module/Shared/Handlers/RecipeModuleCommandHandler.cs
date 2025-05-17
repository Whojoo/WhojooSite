using System.Diagnostics;

using FluentValidation;

using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;

namespace WhojooSite.Recipes.Module.Shared.Handlers;

internal abstract class RecipeModuleCommandHandler<TCommand, TResponse>
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    private readonly RecipesDbContext _dbContext;
    private readonly ILogger<RecipeModuleCommandHandler<TCommand, TResponse>> _logger;
    private readonly IValidator<TCommand>? _validator;

    protected RecipeModuleCommandHandler(
        ILogger<RecipeModuleCommandHandler<TCommand, TResponse>> logger,
        IValidator<TCommand>? validator,
        RecipesDbContext dbContext)
    {
        _logger = logger;
        _validator = validator;
        _dbContext = dbContext;
    }

    protected RecipeModuleCommandHandler(
        ILogger<RecipeModuleCommandHandler<TCommand, TResponse>> logger,
        RecipesDbContext dbContext)
        : this(logger, null, dbContext)
    {
    }

    public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellation)
    {
        _logger.LogInformation("Handling command {CommandName}", typeof(TCommand).Name);
        var startingTimestamp = Stopwatch.GetTimestamp();

        if (_validator is not null)
        {
            // These validations should not do any IO stuff, so use the sync method without cancellation
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation
            var validationResult = _validator.Validate(command);
            if (!validationResult.IsValid)
            {
                var resultErrors = validationResult
                    .Errors
                    .Select(error => new ResultError(error.PropertyName, error.ErrorMessage, ResultStatus.BadRequest))
                    .ToList();

                var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

                _logger.LogWarning(
                    "Command {CommandHandler} failed with status {CommandFailureStatus} in {ElapsedMilliseconds} ms",
                    typeof(TCommand).Name,
                    "InputValidation",
                    elapsed.TotalMilliseconds);

                return Result<TResponse>.Failure(resultErrors);
            }
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellation);
        Result<TResponse> result;

        try
        {
            result = await HandleCommandAsync(command, cancellation);

            await transaction.CommitAsync(cancellation);

            var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

            result.Tap(
                _ => _logger.LogInformation(
                    "Command {CommandHandler} handled successfully in {ElapsedMilliseconds} ms",
                    typeof(TCommand).Name,
                    elapsed.TotalMilliseconds),
                errors => _logger.LogWarning(
                    "Command {CommandHandler} failed with status {CommandFailureStatus} in {ElapsedMilliseconds} ms",
                    typeof(TCommand).Name,
                    errors[0].Status,
                    elapsed.TotalMilliseconds));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling command {CommandName}", typeof(TCommand).Name);
            result = Result<TResponse>.Failure();
        }

        return result;
    }

    protected abstract Task<Result<TResponse>> HandleCommandAsync(TCommand command, CancellationToken cancellation);
}