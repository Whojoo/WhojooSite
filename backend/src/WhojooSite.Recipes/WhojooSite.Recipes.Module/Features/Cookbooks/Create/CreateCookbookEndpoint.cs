using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;

namespace WhojooSite.Recipes.Module.Features.Cookbooks.Create;

internal record CreateCookbookRequest(string Name);

internal class CreateCookbookEndpoint : IRecipeModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder
            .MapPost("/cookbooks", CreateCookbookAsync)
            .WithOpenApi();
    }

    private static async Task<Results<Created, ValidationProblem, InternalServerError>> CreateCookbookAsync(
        [FromBody] CreateCookbookRequest request,
        ICommandHandler<CreateCookbookCommand, CreateCookbookResponse> commandHandler,
        CancellationToken cancellationToken)
    {
        var command = CreateCookbookMapper.RequestToCommand(request);
        var commandResult = await commandHandler.HandleAsync(command, cancellationToken);

        return commandResult.Match<Results<Created, ValidationProblem, InternalServerError>>(
            response => TypedResults.Created($"/api/recipes-module/cookbooks/{response.CookbookId.Value}"),
            errors =>
            {
                if (errors.All(error => error.Status == ResultStatus.BadRequest))
                {
                    return errors.MapToValidationProblem();
                }

                return errors[0].Status switch
                {
                    ResultStatus.BadRequest => errors.MapToValidationProblem(),
                    _ => TypedResults.InternalServerError()
                };
            });
    }
}

[Mapper]
internal static partial class CreateCookbookMapper
{
    public static partial CreateCookbookCommand RequestToCommand(CreateCookbookRequest request);
}