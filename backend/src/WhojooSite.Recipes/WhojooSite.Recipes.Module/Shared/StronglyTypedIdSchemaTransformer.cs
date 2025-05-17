using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Shared;

public class StronglyTypedIdSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type == typeof(RecipeId))
        {
            var targetSchema = new OpenApiSchema { Type = "integer", Format = "int64" };
            schema.Type = targetSchema.Type;
        }

        if (context.JsonTypeInfo.Type == typeof(CookbookId))
        {
            var targetSchema = new OpenApiSchema { Type = "integer", Format = "int64" };
            schema.Type = targetSchema.Type;
        }

        return Task.CompletedTask;
    }
}