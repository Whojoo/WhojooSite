using Dapper;

using FastEndpoints;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using WhojooSite.Common.Cqrs;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence;

namespace WhojooSite.Recipes.Module.Features.Recipes;

internal sealed class ListRecipes(IQueryDispatcher queryDispatcher)
    : Endpoint<ListRecipes.ListRecipesRequest, ListRecipes.ListRecipesResponse>
{
    private readonly IQueryDispatcher _queryDispatcher = queryDispatcher;

    internal class ListRecipesRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    internal record ListRecipesResponse(List<RecipeDto> Recipes);

    public override void Configure()
    {
        Get("/recipes");
        AllowAnonymous();
        Options(x => x.CacheOutput(p => p
            .Expire(TimeSpan.FromMinutes(5))
            .SetVaryByQuery("page", "pageSize")));
    }

    public override async Task HandleAsync(ListRecipesRequest req, CancellationToken ct)
    {
        var query = new ListRecipesQuery(req.Page, req.PageSize);
        var result = await _queryDispatcher.Dispatch<ListRecipesQuery, ListRecipesDto>(query, ct);
        var response = new ListRecipesResponse(result.Recipes);
        await SendOkAsync(response, ct);
    }

    internal record ListRecipesQuery(int Page, int PageSize) : IQuery<ListRecipesDto>;

    internal record ListRecipesDto(List<RecipeDto> Recipes);

    internal record RecipeDto(RecipeId Id, string Name, string Description, CookbookId CookbookId);

    internal class ListRecipesQueryHandler(RecipesDbConnectionFactory connectionFactory)
        : IQueryHandler<ListRecipesQuery, ListRecipesDto>
    {
        private readonly RecipesDbConnectionFactory _connectionFactory = connectionFactory;

        public async ValueTask<ListRecipesDto> Handle(ListRecipesQuery query,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            var limit = query.PageSize;
            var offset = (query.Page - 1) * query.PageSize;

            var recipes = await connection.QueryAsync<RecipeDto>(
                """
                SELECT "Id", "Name", "Description", "CookbookId"
                FROM "Recipes"
                ORDER BY "Id"
                LIMIT @Limit
                OFFSET @Offset
                """,
                new { Limit = limit, Offset = offset });

            return new ListRecipesDto(recipes.ToList());
        }
    }

    internal class ListRecipesValidator : Validator<ListRecipesRequest>
    {
        public ListRecipesValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0);
        }
    }
}