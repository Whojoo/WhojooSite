using Dapper;

using FastEndpoints;

using WhojooSite.Common;
using WhojooSite.Common.Cqrs;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal record GetRecipeByIdRequest(RecipeId Id);

internal record GetRecipeByIdResponse(RecipeId Id, string Name, string Description, CookbookId CookbookId);

internal record GetRecipeByIdQuery(RecipeId RecipeId) : IQuery<Option<GetRecipeByIdResult>>;

internal record GetRecipeByIdResult(RecipeId Id, string Name, string Description, CookbookId CookbookId);

internal class GetRecipeById(IQueryDispatcher queryDispatcher)
    : Endpoint<GetRecipeByIdRequest, GetRecipeByIdResponse>
{
    private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;

    public override void Configure()
    {
        Get("/recipes/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetRecipeByIdRequest req, CancellationToken ct)
    {
        var query = new GetRecipeByIdQuery(req.Id);
        var result = await _queryDispatcher.Dispatch<GetRecipeByIdQuery, Option<GetRecipeByIdResult>>(query, ct);

        await result.MatchAsync(
            notNullAction: async recipe =>
            {
                var response = new GetRecipeByIdResponse(recipe.Id, recipe.Name, recipe.Description, recipe.CookbookId);
                await SendOkAsync(response, ct);
            },
            nullAction: async () =>
            {
                await SendNotFoundAsync(ct);
            });
    }
}

internal class GetRecipeByIdQueryHandler(RecipesDbConnectionFactory connectionFactory)
    : IQueryHandler<GetRecipeByIdQuery, Option<GetRecipeByIdResult>>
{
    private readonly RecipesDbConnectionFactory _connectionFactory = connectionFactory;

    public async ValueTask<Option<GetRecipeByIdResult>> Handle(
        GetRecipeByIdQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<GetRecipeByIdResult>(
            """SELECT "Id", "Name", "Description", "CookbookId" FROM "Recipes" WHERE "Id" = @Id""",
            new { Id = query.RecipeId });
    }
}