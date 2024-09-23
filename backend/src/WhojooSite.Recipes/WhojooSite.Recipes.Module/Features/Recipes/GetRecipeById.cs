using FastEndpoints;

using Microsoft.EntityFrameworkCore;

using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal record GetRecipeByIdRequest(RecipeId Id);

internal record GetRecipeByIdResponse(RecipeId Id, string Name, string Description, CookbookId CookbookId);

internal class GetRecipeById(RecipesDbContext dbContext)
    : Endpoint<GetRecipeByIdRequest, GetRecipeByIdResponse>
{
    private readonly RecipesDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("/recipes/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRecipeByIdRequest req, CancellationToken ct)
    {
        var recipe = await _dbContext
            .Recipes
            .FirstOrDefaultAsync(recipe => recipe.Id == req.Id, ct);

        if (recipe is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetRecipeByIdResponse(recipe.Id, recipe.Name, recipe.Description, recipe.CookbookId);
        await SendOkAsync(response, ct);
    }
}