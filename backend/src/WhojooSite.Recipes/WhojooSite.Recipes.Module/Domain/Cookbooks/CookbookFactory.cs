using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Domain.Cookbooks;

internal static class CookbookFactory
{
    public static Result<Cookbook> Create(string name)
    {
        return Result.Success()
            .ErrorIf(() => string.IsNullOrWhiteSpace(name), CookbookErrors.NameIsRequiredError)
            .Map(() => new Cookbook(name));
    }
}