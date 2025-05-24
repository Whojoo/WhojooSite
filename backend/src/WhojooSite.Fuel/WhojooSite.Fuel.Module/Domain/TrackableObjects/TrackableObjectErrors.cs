using WhojooSite.Common.Results;

namespace WhojooSite.Fuel.Module.Domain.TrackableObjects;

internal static class TrackableObjectErrors
{
    public static readonly ResultError NameRequired = ResultError.BadRequest("TrackableObject.Name", "Name is required");
}