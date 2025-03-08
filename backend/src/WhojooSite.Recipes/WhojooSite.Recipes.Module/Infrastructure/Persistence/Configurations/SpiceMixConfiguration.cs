using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;
using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Infrastructure.Persistence.Configurations;

internal class SpiceMixConfiguration : IEntityTypeConfiguration<SpiceMix>
{
    public void Configure(EntityTypeBuilder<SpiceMix> builder)
    {
        builder.ToTable("SpiceMixes");

        builder.HasKey(spiceMix => spiceMix.Id);

        builder.Ignore(spiceMix => spiceMix.Spices);

        builder
            .Property(spiceMix => spiceMix.Id)
            .HasConversion<SpiceMixId.EfCoreValueConverter>();

        builder
            .Property(spiceMix => spiceMix.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaConstants.NameMaxLength);

        builder.OwnsMany<Ingredient>("_spices", spicesBuilder =>
        {
            spicesBuilder.ToTable("SpiceMixIngredients");

            spicesBuilder
                .WithOwner()
                .HasForeignKey("SpiceMixId");

            spicesBuilder
                .Property(spice => spice.Name)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.NameMaxLength);

            spicesBuilder
                .Property(spice => spice.Amount)
                .IsRequired()
                .HasPrecision(DataSchemaConstants.IngredientPrecision, DataSchemaConstants.IngredientScale);

            spicesBuilder
                .Property(spice => spice.MeasurementUnit)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.MeasurementUnitMaxLength);
        });
    }
}