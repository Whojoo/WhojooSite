using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Persistence.ValueConverters;

namespace WhojooSite.Recipes.Module.Persistence.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");

        builder.HasKey(recipe => recipe.Id);

        builder
            .Property(recipe => recipe.Id)
            .HasConversion(
                recipeId => recipeId.Value,
                value => new RecipeId(value));

        builder
            .Property(recipe => recipe.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaConstants.NameMaxLength);

        builder
            .Property(recipe => recipe.Description)
            .IsRequired()
            .HasMaxLength(DataSchemaConstants.DescriptionMaxLength);

        builder
            .Property(recipe => recipe.OwnerId)
            .IsRequired()
            .HasConversion<OwnerIdConverter>();

        builder
            .Property(recipe => recipe.CookbookId)
            .IsRequired()
            .HasConversion<CookbookIdConverter>();

        builder.OwnsMany<Step>("_steps", stepsBuilder =>
        {
            stepsBuilder.ToTable("Steps");

            stepsBuilder.HasKey(step => step.Id);

            stepsBuilder
                .WithOwner()
                .HasForeignKey(step => step.RecipeId);

            stepsBuilder
                .Property(step => step.Id)
                .HasConversion<StepIdConverter>();

            stepsBuilder
                .Property(step => step.Name)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.NameMaxLength);

            stepsBuilder
                .Property(step => step.Summary)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.SummaryMaxLength);

            stepsBuilder
                .Property(step => step.RecipeId)
                .HasConversion<RecipeIdConverter>();
        });

        builder.OwnsMany<Ingredient>("_ingredients", spicesBuilder =>
        {
            spicesBuilder.ToTable("RecipeIngredients");

            spicesBuilder
                .WithOwner()
                .HasForeignKey("RecipeId");

            spicesBuilder
                .Property(ingredient => ingredient.Name)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.NameMaxLength);

            spicesBuilder
                .Property(ingredient => ingredient.Amount)
                .IsRequired()
                .HasPrecision(DataSchemaConstants.IngredientPrecision, DataSchemaConstants.IngredientScale);

            spicesBuilder
                .Property(ingredient => ingredient.MeasurementUnit)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.MeasurementUnitMaxLength);
        });

        builder.OwnsMany<Ingredient>("_spices", spicesBuilder =>
        {
            spicesBuilder.ToTable("RecipeSpices");

            spicesBuilder
                .WithOwner()
                .HasForeignKey("RecipeId");

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

        builder.OwnsMany<SpiceMixIngredient>("_spiceMixIngredients", spiceMixIngredientsBuilder =>
        {
            spiceMixIngredientsBuilder.ToTable("RecipeSpiceMixIngredients");

            spiceMixIngredientsBuilder
                .WithOwner()
                .HasForeignKey("RecipeId");

            spiceMixIngredientsBuilder
                .Property(spiceMixIngredient => spiceMixIngredient.SpiceMixId)
                .HasConversion<SpiceMixIdConverter>()
                .IsRequired();

            spiceMixIngredientsBuilder
                .Property(spiceMixIngredient => spiceMixIngredient.Amount)
                .IsRequired()
                .HasPrecision(DataSchemaConstants.IngredientPrecision, DataSchemaConstants.IngredientScale);

            spiceMixIngredientsBuilder
                .Property(spiceMixIngredient => spiceMixIngredient.MeasurementUnit)
                .IsRequired()
                .HasMaxLength(DataSchemaConstants.MeasurementUnitMaxLength);
        });
    }
}