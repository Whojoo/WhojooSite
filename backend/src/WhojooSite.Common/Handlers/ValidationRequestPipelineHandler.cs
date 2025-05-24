using FluentValidation;

using WhojooSite.Common.Results;

namespace WhojooSite.Common.Handlers;

public class ValidationRequestPipelineHandler<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IRequestPipelineHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<Result<TResponse>> HandlerAsync(
        TRequest request,
        Func<TRequest, CancellationToken, Task<Result<TResponse>>> next,
        CancellationToken cancellation)
    {
        if (_validator is null)
        {
            return await next(request, cancellation);
        }

        // These validators should be sync, so we can ignore this warning
        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
        var validationResult = _validator.Validate(request);

        if (validationResult.IsValid)
        {
            return await next(request, cancellation);
        }

        var resultErrors = validationResult
            .Errors
            .Select(error => new ResultError(error.PropertyName, error.ErrorMessage, ResultStatus.BadRequest))
            .ToList();

        return Result<TResponse>.Failure(resultErrors);
    }
}