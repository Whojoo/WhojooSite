using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Persistence.Configurations;

public class SpiceMixConfiguration : IEntityTypeConfiguration<SpiceMix>
{
    public void Configure(EntityTypeBuilder<SpiceMix> builder)
    {

    }
}