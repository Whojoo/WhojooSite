using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Domain.Cookbooks;

internal static class CookbookErrors
{
    public static ResultError NameIsRequiredError = ResultError.BadRequest("Cookbook.Name", "Name is required");
}