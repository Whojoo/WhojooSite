using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Shared.Dtos;

internal record ListTrackableObjectsResultDto(TrackableObject[] TrackableObjects, int TotalCount);