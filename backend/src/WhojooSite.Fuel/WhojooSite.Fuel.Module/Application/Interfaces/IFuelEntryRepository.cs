using WhojooSite.Fuel.Module.Domain.FuelEntries;

namespace WhojooSite.Fuel.Module.Application.Interfaces;

internal interface IFuelEntryRepository
{
    Task<FuelEntry?> GetAsync(FuelEntryId id, CancellationToken cancellationToken);

    Task<int> StoreAsync(FuelEntry fuelEntry, CancellationToken cancellationToken);
}