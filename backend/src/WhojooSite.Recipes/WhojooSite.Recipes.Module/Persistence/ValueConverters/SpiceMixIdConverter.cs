using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Persistence.ValueConverters;

public class SpiceMixIdConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<SpiceMixId, long>(
        spiceMixId => spiceMixId.Value,
        value => new SpiceMixId(value),
        mappingHints);