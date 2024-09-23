using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

[JsonConverter(typeof(OwnerIdJsonConverter))]
internal class OwnerId(Guid id) : TypedId<Guid>(id)
{
    public static OwnerId Empty { get; } = new();

    private OwnerId() : this(Guid.Empty)
    {
    }
}

internal class OwnerIdJsonConverter : JsonConverter<OwnerId>
{
    public override OwnerId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetGuid();
        return new OwnerId(id);
    }

    public override void Write(Utf8JsonWriter writer, OwnerId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}

internal class OwnerIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<OwnerId, Guid>(
        ownerId => ownerId.Value,
        value => new OwnerId(value),
        mappingHints)
{
    public OwnerIdValueConverter() : this(null)
    {
    }
}