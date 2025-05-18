using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Shared.Dtos;

internal record RecipeDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);