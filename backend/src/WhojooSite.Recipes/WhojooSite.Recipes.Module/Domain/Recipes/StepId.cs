using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

[JsonConverter(typeof(StepIdJsonConverter))]
internal class StepId(long id) : TypedId<long>(id)
{
    public static StepId Empty => new();

    private StepId() : this(default)
    {
    }
}

internal class StepIdJsonConverter : JsonConverter<StepId>
{
    public override StepId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetInt64();
        return new StepId(id);
    }

    public override void Write(Utf8JsonWriter writer, StepId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

internal class StepIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<StepId, long>(
        stepId => stepId.Value,
        value => new StepId(value),
        mappingHints)
{
    public StepIdValueConverter() : this(null)
    {
    }
}