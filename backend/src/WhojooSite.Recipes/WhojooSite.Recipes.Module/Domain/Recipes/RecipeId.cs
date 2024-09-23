using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Common;

namespace WhojooSite.Recipes.Module.Domain.Recipes;

[JsonConverter(typeof(RecipeIdJsonConverter))]
internal class RecipeId(long id) : TypedId<long>(id)
{
    public static RecipeId Empty { get; } = new();

    private RecipeId() : this(default)
    {
    }
}

internal class RecipeIdJsonConverter : JsonConverter<RecipeId>
{
    public override RecipeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var id = reader.GetInt64();
        return new RecipeId(id);
    }

    public override void Write(Utf8JsonWriter writer, RecipeId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}

internal class RecipeIdValueConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<RecipeId, long>(
        recipeId => recipeId.Value,
        value => new RecipeId(value),
        mappingHints)
{
    public RecipeIdValueConverter() : this(null)
    {
    }
}