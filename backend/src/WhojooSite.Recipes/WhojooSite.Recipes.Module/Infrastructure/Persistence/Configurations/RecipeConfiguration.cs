using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using WhojooSite.Recipes.Module.Domain.Common.StronglyTypedIds;
using WhojooSite.Recipes.Module.Domain.Common.ValueObjects;
using WhojooSite.Recipes.Module.Domain.Cookbook;
using WhojooSite.Recipes.Module.Domain.Recipes;
using WhojooSite.Recipes.Module.Domain.SpiceMix;

namespace WhojooSite.Recipes.Module.Infrastructure.Persistence.Configurations;

internal class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");

        builder.HasKey(recipe => recipe.Id);

        builder.Ignore(recipe => recipe.Ingredients);
        builder.Ignore(recipe => recipe.Spices);
        builder.Ignore(recipe => recipe.SpiceMixIngredients);
        builder.Ignore(recipe => recipe.Steps);

        builder
            .Property(recipe => recipe.Id)
            .HasConversion<RecipeId.EfCoreValueConverter>();

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
            .HasConversion<OwnerId.EfCoreValueConverter>();

        builder
            .Property(recipe => recipe.CookbookId)
            .IsRequired()
            .HasConversion<CookbookId.EfCoreValueConverter>();

        builder.OwnsMany<Step>("_steps", stepsBuilder =>
        {
            stepsBuilder.ToTable("Steps");

            stepsBuilder.HasKey(step => step.Id);

            stepsBuilder
                .WithOwner()
                .HasForeignKey(step => step.RecipeId);

            stepsBuilder
                .Property(step => step.Id)
                .HasConversion<StepId.EfCoreValueConverter>();

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
                .HasConversion<RecipeId.EfCoreValueConverter>();
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
                .HasConversion<SpiceMixId.EfCoreValueConverter>()
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