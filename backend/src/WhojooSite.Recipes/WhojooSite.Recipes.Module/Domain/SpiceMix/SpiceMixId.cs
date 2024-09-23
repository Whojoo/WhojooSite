using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.SpiceMix;

[JsonConverter(typeof(SpiceMixIdJsonConverter))]
internal class SpiceMixId(long id) : TypedId<long>(id)
{
    public static SpiceMixId Empty { get; } = new();

    private SpiceMixId() : this(default)
    {
    }
}

internal class SpiceMixIdJsonConverter : JsonConverter<SpiceMixId>
{
    public override SpiceMixId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetInt64();
        return new SpiceMixId(id);
    }

    public override void Write(Utf8JsonWriter writer, SpiceMixId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

internal class SpiceMixIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<SpiceMixId, long>(
        spiceMixId => spiceMixId.Value,
        value => new SpiceMixId(value),
        mappingHints)
{
    public SpiceMixIdValueConverter() : this(null)
    {
    }
}