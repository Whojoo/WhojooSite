namespace WhojooSite.Recipes.Module.Domain.Common.ValueObjects;

public class Ingredient : ValueObject<Ingredient>
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

    protected override bool IsEqualTo(Ingredient other)
    {
        const double precision = 0.000001;

        return string.Equals(Name, other.Name) &&
               string.Equals(MeasurementUnit, other.MeasurementUnit) &&
               Math.Abs(Amount - other.Amount) < precision;
    }

    protected override int CalculateHashCode()
    {
        return HashCode.Combine(Name, MeasurementUnit, Amount);
    }
}