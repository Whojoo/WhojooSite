﻿using Grpc.Core;

using WhojooSite.Common.Protos;
using WhojooSite.Recipes.Module.Application.Recipes;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.RecipesModule.Protos.Recipes;

using Recipe = WhojooSite.RecipesModule.Protos.Recipes.Recipe;

namespace WhojooSite.Recipes.Module.Grpc;

internal class RecipeService(GetByIdHandler getByIdHandler)
    : RecipesModule.Protos.Recipes.RecipeService.RecipeServiceBase
{
    private readonly GetByIdHandler _getByIdHandler = getByIdHandler;

    public override async Task<RecipeByIdResponse> GetRecipeById(RecipeByIdRequest request, ServerCallContext context)
    {
        var query = new GetByIdQuery(new RecipeId(request.Id.Id));
        var result = await _getByIdHandler.HandleAsync(query).ConfigureAwait(false);

        return result.Match(
            successResult => new RecipeByIdResponse
            {
                Recipe = new Recipe { Id = new WhojooSiteRecipes.Module.Protos.Recipes.RecipeId { Id = successResult.Recipe.Id.Value } }
            },
            errors =>
            {
                var response = new RecipeByIdResponse { FailureResult = new OperationFailureResult { Status = FailureStatus.BadRequest } };
                response.FailureResult.Errors.AddRange(errors.Select(error =>
                {
                    var responseError = new Error { Code = error.Code };
                    responseError.Descriptions.AddRange(error.Description.Select(desc => new ErrorDescription { Description = desc }));
                    return responseError;
                }));

                return response;
            });
    }
}