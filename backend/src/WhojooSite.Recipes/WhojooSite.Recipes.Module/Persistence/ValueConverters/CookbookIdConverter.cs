using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Cookbook;

namespace WhojooSite.Recipes.Module.Persistence.ValueConverters;

public class CookbookIdConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<CookbookId, long>(
        cookbookId => cookbookId.Value,
        value => new CookbookId(value),
        mappingHints)
{
    public CookbookIdConverter() : this(null)
    {
    }
}