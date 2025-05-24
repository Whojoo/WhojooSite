using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Application.Interfaces;

internal interface IObjectTypeRepository
{
    Task<ObjectTypeId?> GetIdFromNameAsync(string name, CancellationToken cancellationToken);
}