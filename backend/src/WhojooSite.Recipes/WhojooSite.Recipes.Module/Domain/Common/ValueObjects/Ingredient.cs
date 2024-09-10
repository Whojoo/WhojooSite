namespace WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

public sealed class Ingredient(string name, double amount, string measurementUnit) : ValueObject
{
    public string Name { get; init; } = name;

    public double Amount { get; init; } = amount;

    public string MeasurementUnit { get; init; } = measurementUnit;

    protected override object[] GetEqualityComponents()
    {
        return [Name, Amount, MeasurementUnit];
    }
}