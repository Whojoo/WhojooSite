using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Recipes;

namespace WhojooSite.Recipes.Module.Persistence.ValueConverters;

public class StepIdConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<StepId, long>(
        stepId => stepId.Value,
        value => new StepId(value),
        mappingHints)
{
    public StepIdConverter() : this(null)
    {
    }
}