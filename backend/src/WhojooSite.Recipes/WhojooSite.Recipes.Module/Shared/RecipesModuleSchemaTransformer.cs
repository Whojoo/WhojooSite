using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Shared;

public class RecipesModuleSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var longSchema = new OpenApiSchema { Type = "integer", Format = "int64" };

        if (context.JsonTypeInfo.Type == typeof(RecipeId))
        {
            schema.Type = longSchema.Type;
            schema.Format = longSchema.Format;
        }

        if (context.JsonTypeInfo.Type == typeof(CookbookId))
        {
            schema.Type = longSchema.Type;
            schema.Format = longSchema.Format;
        }

        if (context.JsonTypeInfo.Type == typeof(OwnerId))
        {
            var guidSchema = new OpenApiSchema { Type = "string", Format = "uuid" };
            schema.Type = guidSchema.Type;
            schema.Format = guidSchema.Format;
        }

        return Task.CompletedTask;
    }
}