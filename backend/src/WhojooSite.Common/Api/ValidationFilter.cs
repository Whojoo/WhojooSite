using FluentValidation;

using Microsoft.AspNetCore.Http;

namespace WhojooSite.Common.Api;

internal class ValidationFilter<T>(IValidator<T> validator, int argumentIndex) : IEndpointFilter
{
    private readonly IValidator<T> _validator = validator;
    private readonly int _argumentIndex = argumentIndex;

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validationTarget = context.GetArgument<T>(_argumentIndex);

        var validationResult = _validator.Validate(validationTarget);

        if (validationResult.IsValid)
        {
            return next(context);
        }

        var validationProblem = TypedResults.ValidationProblem(validationResult.ToDictionary());
        return ValueTask.FromResult<object?>(validationProblem);
    }
}