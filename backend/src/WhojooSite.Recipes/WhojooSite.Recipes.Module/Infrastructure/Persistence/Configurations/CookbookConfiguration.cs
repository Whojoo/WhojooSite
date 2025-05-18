using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.Cookbooks;
using WhojooSite.Recipes.Module.Domain.Recipes;

using CookbookId = WhojooSite.Recipes.Module.Domain.Cookbooks.CookbookId;

namespace WhojooSite.Recipes.Module.Infrastructure.Persistence.Configurations;

internal class CookbookConfiguration : IEntityTypeConfiguration<Cookbook>
{
    public void Configure(EntityTypeBuilder<Cookbook> builder)
    {
        builder.ToTable("Cookbooks");

        builder.HasKey(cookbook => cookbook.Id);

        builder.Ignore(cookbook => cookbook.RecipeIds);

        builder
            .Property(cookbook => cookbook.Id)
            .HasConversion<CookbookId.EfCoreValueConverter>();

        builder
            .Property(cookbook => cookbook.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(cookbook => cookbook.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaConstants.NameMaxLength);

        builder
            .Property<List<RecipeId>>("_recipeIds")
            .HasConversion(
                recipeIds => JsonSerializer.Serialize(recipeIds, JsonSerializerOptions.Default),
                value => JsonSerializer.Deserialize<List<RecipeId>>(value, JsonSerializerOptions.Default)!)
            .Metadata
            .SetValueComparer(new ValueComparer<List<RecipeId>>(
                (left, right) => left!.SequenceEqual(right!),
                list => list.Aggregate(0,
                    (accumulated, recipeId) => HashCode.Combine(accumulated, recipeId.GetHashCode())),
                list => list.ToList()));
    }
}