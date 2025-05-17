using FluentValidation;

namespace WhojooSite.Recipes.Module.Features.Recipes.List;

internal class ListRecipesRequestValidator : AbstractValidator<ListRecipesQuery>
{
    public ListRecipesRequestValidator()
    {
        RuleFor(request => request.PageSize)
            .GreaterThan(0);

        RuleFor(request => request.NextKey)
            .GreaterThanOrEqualTo(0)
            .When(request => request.NextKey.HasValue);
    }
}