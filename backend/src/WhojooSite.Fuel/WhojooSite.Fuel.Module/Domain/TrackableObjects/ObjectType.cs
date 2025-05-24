namespace WhojooSite.Fuel.Module.Domain.TrackableObjects;

internal class ObjectType(ObjectTypeId objectTypeId, string name, DateTimeOffset creationDate)
{
    private ObjectType() : this(ObjectTypeId.Empty, string.Empty, DateTimeOffset.MinValue)
    {
    }

    public ObjectTypeId ObjectTypeId { get; init; } = objectTypeId;
    public string Name { get; init; } = name;
    public DateTimeOffset CreationDate { get; init; } = creationDate;
}