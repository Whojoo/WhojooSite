using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Domain.FuelEntries;

internal partial class FuelEntry(
    int mileage,
    DateTimeOffset creationDate,
    double refuelAmount,
    TrackableObjectId objectId,
    DateTimeOffset? executionDate = null,
    decimal? literPrice = null,
    decimal? totalCost = null,
    FuelEntryId? id = null)
{
    public FuelEntryId Id { get; private set; } = id ?? new FuelEntryId(Guid.CreateVersion7());
    public int Mileage { get; private set; } = mileage;
    public DateTimeOffset CreationDate { get; private set; } = creationDate;
    public DateTimeOffset? ExecutionDate { get; private set; } = executionDate;
    public double RefuelAmount { get; } = refuelAmount;
    public decimal? LiterPrice { get; private set; } = literPrice;
    public decimal? TotalCost { get; private set; } = totalCost;

    public TrackableObjectId ObjectId { get; private set; } = objectId;
    public TrackableObject? TrackableObject { get; set; }

    public Result AddLiterPrice(decimal literPrice)
    {
        if (literPrice < 0)
        {
            return Result.Failure(FuelEntryErrors.LiterPriceMustBePositiveError);
        }

        LiterPrice = literPrice;
        TotalCost = literPrice * (decimal)RefuelAmount;

        return Result.Success();
    }

    public Result AddTotalCost(decimal totalCost)
    {
        if (totalCost < 0)
        {
            return Result.Failure(FuelEntryErrors.TotalCostMustBePositiveError);
        }

        TotalCost = totalCost;
        LiterPrice = totalCost / (decimal)RefuelAmount;

        return Result.Success();
    }
}