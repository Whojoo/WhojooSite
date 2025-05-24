using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

using WhojooSite.Fuel.Module.Domain.FuelEntries;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module;

public class FuelModuleSchemaTransformer : IOpenApiSchemaTransformer
{
    private static readonly Type[] GuidIdTypes =
    [
        typeof(FuelEntryId),
        typeof(TrackableObjectId),
        typeof(ObjectTypeId),
        typeof(OwnerId)
    ];

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var guidSchema = new OpenApiSchema { Type = "string", Format = "uuid" };

        if (GuidIdTypes.Contains(context.JsonTypeInfo.Type))
        {
            schema.Type = guidSchema.Type;
            schema.Format = guidSchema.Format;
        }

        return Task.CompletedTask;
    }
}