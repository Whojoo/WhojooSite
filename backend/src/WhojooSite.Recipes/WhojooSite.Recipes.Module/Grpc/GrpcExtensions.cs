using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace WhojooSite.Recipes.Module.Grpc;

public static class GrpcExtensions
{
    public static IEndpointRouteBuilder MapGrpcServices(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGrpcService<RecipeService>();

        return endpointRouteBuilder;
    }
}