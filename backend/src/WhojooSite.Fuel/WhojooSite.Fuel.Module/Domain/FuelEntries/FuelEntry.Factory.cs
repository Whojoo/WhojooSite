using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Domain.FuelEntries;

internal partial class FuelEntry
{
    public static class Factory
    {
        public static Result<FuelEntry> CreateNew(
            int mileage,
            double refuelAmount,
            TrackableObjectId objectId,
            DateTimeOffset? executionDate = null,
            TrackableObject? trackableObject = null)
        {
            var result = Result.Success();

            if (mileage < 0)
            {
                result.WithError(FuelEntryErrors.MileageMustBePositiveError);
            }

            if (refuelAmount < 0)
            {
                result.WithError(FuelEntryErrors.RefuelAmountMustBePositiveError);
            }

            if (objectId.Value == Guid.Empty)
            {
                result.WithError(FuelEntryErrors.TrackingObjectIdIsRequiredError);
            }

            return result.Map(() =>
            {
                var fuelEntry = new FuelEntry(
                    mileage,
                    DateTimeOffset.UtcNow,
                    refuelAmount,
                    objectId,
                    executionDate);

                fuelEntry.TrackableObject = trackableObject;

                return fuelEntry;
            });
        }
    }
}