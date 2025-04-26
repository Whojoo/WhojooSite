using FluentValidation;

using Microsoft.AspNetCore.Http;

namespace WhojooSite.Common.Api;

internal class ValidationFilter<T>(IValidator<T> validator, int argumentIndex) : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validationTarget = context.GetArgument<T>(argumentIndex);

        var validationResult = validator.Validate(validationTarget);

        if (validationResult.IsValid)
        {
            return next(context);
        }

        var validationProblem = TypedResults.ValidationProblem(validationResult.ToDictionary());
        return ValueTask.FromResult<object?>(validationProblem);
    }
}