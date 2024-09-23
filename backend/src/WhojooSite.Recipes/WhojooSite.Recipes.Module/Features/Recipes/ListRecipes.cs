using FastEndpoints;

using Microsoft.EntityFrameworkCore;

using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal sealed record ListRecipesResponse(List<Recipe> Recipes);

internal sealed class ListRecipes(RecipesDbContext recipesDbContext)
    : EndpointWithoutRequest<ListRecipesResponse>
{
    private readonly RecipesDbContext _recipesDbContext = recipesDbContext;

    public override void Configure()
    {
        Get("/recipes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var recipes = await _recipesDbContext.Recipes.ToListAsync(ct);
        await SendOkAsync(new ListRecipesResponse(recipes), ct);
    }
}