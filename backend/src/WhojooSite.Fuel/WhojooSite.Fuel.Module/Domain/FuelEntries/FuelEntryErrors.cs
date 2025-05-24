using WhojooSite.Common.Results;

namespace WhojooSite.Fuel.Module.Domain.FuelEntries;

internal static class FuelEntryErrors
{
    public static readonly ResultError MileageMustBePositiveError = ResultError.BadRequest(
        "FuelEntry.Mileage",
        "Mileage must be positive");

    public static readonly ResultError RefuelAmountMustBePositiveError = ResultError.BadRequest(
        "FuelEntry.RefuelAmount",
        "Refuel amount must be positive");

    public static readonly ResultError TrackingObjectIdIsRequiredError = ResultError.BadRequest(
        "FuelEntry.TrackingObjectId",
        "Tracking object id is required");

    public static readonly ResultError LiterPriceMustBePositiveError = ResultError.BadRequest(
        "FuelEntry.LiterPrice",
        "Liter price must be positive");

    public static readonly ResultError TotalCostMustBePositiveError = ResultError.BadRequest(
        "FuelEntry.TotalCost",
        "Total cost must be positive");
}