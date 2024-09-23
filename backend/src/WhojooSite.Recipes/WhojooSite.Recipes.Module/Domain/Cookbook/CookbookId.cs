using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Cookbook;

[JsonConverter(typeof(CookbookIdJsonConverter))]
internal class CookbookId(long id) : TypedId<long>(id)
{
    public static CookbookId Empty = new();

    private CookbookId() : this(default)
    {
    }
}

internal class CookbookIdJsonConverter : JsonConverter<CookbookId>
{
    public override CookbookId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetInt64();
        return new CookbookId(id);
    }

    public override void Write(Utf8JsonWriter writer, CookbookId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

internal class CookbookIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<CookbookId, long>(
        cookbookId => cookbookId.Value,
        value => new CookbookId(value),
        mappingHints)
{
    public CookbookIdValueConverter() : this(null)
    {
    }
}