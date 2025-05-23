namespace WhojooSite.Recipes.Module.Infrastructure.Persistence;

public static class DataSchemaConstants
{
    public const string ConnectionStringName = "RecipesDb";

    public const int NameMaxLength = 200;
    public const int MeasurementUnitMaxLength = 20;
    public const int DescriptionMaxLength = 500;
    public const int SummaryMaxLength = 500;

    public const int IngredientPrecision = 6;
    public const int IngredientScale = 2;
}