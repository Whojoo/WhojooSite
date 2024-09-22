using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Persistence.ValueConverters;

public class RecipeIdConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<RecipeId, long>(
        recipeId => recipeId.Value,
        value => new RecipeId(value),
        mappingHints)
{
    public RecipeIdConverter() : this(null)
    {
    }
}