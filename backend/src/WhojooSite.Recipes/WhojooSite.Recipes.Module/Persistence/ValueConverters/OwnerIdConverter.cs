using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;

namespace WhojooSite.Recipes.Module.Persistence.ValueConverters;

public class OwnerIdConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<OwnerId, Guid>(
        ownerId => ownerId.Value,
        value => new OwnerId(value),
        mappingHints);