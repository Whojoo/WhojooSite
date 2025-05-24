using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Domain.Shared;

namespace WhojooSite.Fuel.Module.Domain.TrackableObjects;

internal class TrackableObject(
    string name,
    ObjectTypeId objectTypeId,
    OwnerId ownerId,
    DateTimeOffset creationDate,
    TrackableObjectId? id = null)
{
    private TrackableObject() : this(string.Empty, ObjectTypeId.Empty, OwnerId.Empty, DateTimeOffset.MinValue)
    {
    }

    public TrackableObjectId Id { get; init; } = id ?? new TrackableObjectId(Guid.CreateVersion7());
    public string Name { get; private set; } = name;
    public OwnerId OwnerId { get; init; } = ownerId;
    public DateTimeOffset CreationDate { get; init; } = creationDate;

    public ObjectTypeId ObjectTypeId { get; init; } = objectTypeId;
    public ObjectType? ObjectType { get; set; }

    public static class Factory
    {
        public static Result<TrackableObject> Create(string name, ObjectTypeId objectTypeId, OwnerId ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<TrackableObject>.Failure(TrackableObjectErrors.NameRequired);
            }

            var trackableObject = new TrackableObject(name, objectTypeId, ownerId, DateTimeOffset.UtcNow);
            return Result.Success(trackableObject);
        }
    }
}