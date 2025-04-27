using WhojooSite.Common;
using WhojooSite.RecipesModule.Protos.Recipes;
using WhojooSite.View.Application.Recipes.Client;

using WhojooSiteRecipes.Module.Protos.Recipes;

namespace WhojooSite.View.Infrastructure.Recipes;

public class GrpcRecipesClient(RecipeService.RecipeServiceClient client) : IRecipesClient
{
    private readonly RecipeService.RecipeServiceClient _client = client;

    public async Task<Result<RecipeDto>> GetRecipeByIdAsync(long id)
    {
        var request = new RecipeByIdRequest { Id = new RecipeId { Id = id } };

        var response = await _client.GetRecipeByIdAsync(request);

        if (response is null)
        {
            return Result.Failure<RecipeDto>();
        }

        return response.ResultCase switch
        {
            RecipeByIdResponse.ResultOneofCase.None => Result.Failure<RecipeDto>(),
            RecipeByIdResponse.ResultOneofCase.Recipe => Result.Success(new RecipeDto(response.Recipe.Id.Id)),
            RecipeByIdResponse.ResultOneofCase.FailureResult => response.FailureResult.MapToFailureResult<RecipeDto>(),
            _ => Result.Failure<RecipeDto>()
        };
    }

    public Task<Result<List<RecipeDto>>> ListRecipesAsync(ListRecipesRequest request)
    {
        return Task.FromResult(Result.Failure<List<RecipeDto>>());
    }
}