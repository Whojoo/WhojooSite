using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence.ValueConverters;

namespace WhojooSite.Recipes.Module.Persistence.Configurations;

public class CookbookConfiguration : IEntityTypeConfiguration<Cookbook>
{
    public void Configure(EntityTypeBuilder<Cookbook> builder)
    {
        builder.ToTable("Cookbooks");

        builder.HasKey(cookbook => cookbook.Id);

        builder
            .Property(cookbook => cookbook.Id)
            .HasConversion<CookbookIdConverter>();

        builder
            .Property(cookbook => cookbook.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaConstants.NameMaxLength);

        builder
            .Property<List<RecipeId>>("_recipeIds")
            .HasConversion(
                recipeIds => JsonSerializer.Serialize(recipeIds, JsonSerializerOptions.Default),
                value => JsonSerializer.Deserialize<List<RecipeId>>(value, JsonSerializerOptions.Default)!);
    }
}