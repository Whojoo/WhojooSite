using Dapper;

using FastEndpoints;

using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common;
using WhojooSite.Common.Cqrs;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal class GetRecipeById(IQueryDispatcher queryDispatcher)
    : Endpoint<GetRecipeById.GetRecipeByIdRequest, GetRecipeById.GetRecipeByIdResponse>
{
    private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;

    internal record GetRecipeByIdRequest(RecipeId Id);

    internal record GetRecipeByIdResponse(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    public override void Configure()
    {
        Get("/recipes/{Id}");
        AllowAnonymous();
        Options(x =>
        {
            x.CacheOutput(p => p.Expire(TimeSpan.FromSeconds(5)));
        });
    }

    public override async Task HandleAsync(GetRecipeByIdRequest req, CancellationToken ct)
    {
        var query = new GetRecipeByIdQuery(req.Id);
        var result = await _queryDispatcher.Dispatch<GetRecipeByIdQuery, Option<RecipeDto>>(query, ct);

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

    internal record GetRecipeByIdQuery(RecipeId RecipeId) : IQuery<Option<RecipeDto>>;

    internal record RecipeDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    internal class GetRecipeByIdQueryHandler(RecipesDbConnectionFactory connectionFactory)
        : IQueryHandler<GetRecipeByIdQuery, Option<RecipeDto>>
    {
        private readonly RecipesDbConnectionFactory _connectionFactory = connectionFactory;

        public async ValueTask<Option<RecipeDto>> Handle(
            GetRecipeByIdQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<RecipeDto>(
                """SELECT "Id", "Name", "Description", "CookbookId" FROM "Recipes" WHERE "Id" = @Id""",
                new { Id = query.RecipeId });
        }
    }
}