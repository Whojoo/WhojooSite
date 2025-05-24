using WhojooSite.Fuel.Module.Domain.TrackableObjects;
using WhojooSite.Fuel.Module.Shared.Dtos;

namespace WhojooSite.Fuel.Module.Application.Interfaces;

internal interface ITrackableObjectRepository
{
    Task<TrackableObject?> GetAsync(TrackableObjectId id, CancellationToken cancellationToken);
    Task<int> StoreAsync(TrackableObject trackableObject, CancellationToken cancellationToken);
    Task<ListTrackableObjectsResultDto> ListAsync(CancellationToken cancellationToken);
}