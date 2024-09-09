using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Web;

namespace WhojooSite.Recipes.Module.Recipes;

public static class GetRecipes
{
    internal sealed class Endpoint : IEndpoint
    {
        public void AddEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/recipes", () => "Hello Recipes");
        }
    }
    
}