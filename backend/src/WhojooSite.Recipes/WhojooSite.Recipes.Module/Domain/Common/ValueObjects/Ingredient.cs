namespace WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

public sealed class Ingredient : ValueObject
{
    public string Name { get; init; } = string.Empty;

    public double Amount { get; init; }

    public string MeasurementUnit { get; init; } = string.Empty;

    public Ingredient(string name, double amount, string measurementUnit)
    {
        Name = name;
        Amount = amount;
        MeasurementUnit = measurementUnit;
    }

    private Ingredient() { }

    protected override object[] GetEqualityComponents()
    {
        return [Name, Amount, MeasurementUnit];
    }
}