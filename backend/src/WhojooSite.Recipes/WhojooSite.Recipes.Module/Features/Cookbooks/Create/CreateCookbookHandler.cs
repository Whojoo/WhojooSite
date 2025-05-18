using Microsoft.Extensions.Logging;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Infrastructure.Persistence;
using WhojooSite.Recipes.Module.Shared.Handlers;

namespace WhojooSite.Recipes.Module.Features.Cookbooks.Create;

internal record CreateCookbookCommand(string Name) : ICommand<CreateCookbookResponse>;

internal record CreateCookbookResponse(CookbookId CookbookId);

internal class CreateCookbookHandler(
    ILogger<RecipeModuleCommandHandler<CreateCookbookCommand, CreateCookbookResponse>> logger,
    RecipesDbContext dbContext)
    : RecipeModuleCommandHandler<CreateCookbookCommand, CreateCookbookResponse>(logger, dbContext)
{
    private readonly RecipesDbContext _dbContext = dbContext;

    protected override async Task<Result<CreateCookbookResponse>> HandleCommandAsync(
        CreateCookbookCommand command,
        CancellationToken cancellation)
    {
        return await CookbookFactory.Create(command.Name)
            .MapAsync(cookbook => StoreCookbookAsync(cookbook, cancellation))
            .MapAsync(cookbookId => new CreateCookbookResponse(cookbookId));
    }

    private async Task<CookbookId> StoreCookbookAsync(Cookbook cookbook, CancellationToken cancellation)
    {
        await _dbContext.Cookbooks.AddAsync(cookbook, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return cookbook.Id;
    }
}