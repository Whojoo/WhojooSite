using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Features.Recipes.StartNew;

internal record StartNewRecipeRequest(string Name, string Description, OwnerId OwnerId, CookbookId CookbookId);

internal class StartNewRecipe : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapPost("/recipes/start-new-recipe", StartNewRecipeAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Created, ValidationProblem>> StartNewRecipeAsync(
        [FromBody] StartNewRecipeRequest request,
        ICommandHandler<StartNewRecipeCommand, RecipeId> commandHandler,
        CancellationToken cancellationToken)
    {
        var command = StartNewRecipeMapper.RequestToCommand(request);
        var commandResult = await commandHandler.HandleAsync(command, cancellationToken);

        return commandResult.Match<Results<Created, ValidationProblem>>(
            recipeId => TypedResults.Created($"/api/recipes-module/recipes/{recipeId.Value}"),
            errors => errors.MapToValidationProblem());
    }
}

[Mapper]
internal static partial class StartNewRecipeMapper
{
    public static StartNewRecipeCommand RequestToCommand(StartNewRecipeRequest request)
    {
        return new StartNewRecipeCommand(RequestToNewRecipeModel(request));
    }

    private static partial NewRecipe RequestToNewRecipeModel(StartNewRecipeRequest request);
}