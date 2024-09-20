using FastEndpoints;

using Microsoft.Extensions.Logging;

namespace WhojooSite.Recipes.Module.Recipes;

internal sealed class GetRecipes : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/recipes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync("Hello, World!", cancellation: ct);
    }
}