using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Application.TrackableObjects.Create;

internal record CreateTrackableObjectCommand(string Name, OwnerId OwnerId, string ObjectTypeName) : ICommand<TrackableObjectId>;

internal class CreateTrackableObjectHandler(
    ITrackableObjectRepository trackableObjectRepository,
    IObjectTypeRepository objectTypeRepository)
    : ICommandHandler<CreateTrackableObjectCommand, TrackableObjectId>
{
    private static readonly ResultError InvalidObjectType = ResultError.BadRequest("ObjectTypeName", "Invalid object type name");

    private static readonly ResultError StoringTrackableObjectFailed =
        ResultError.Failure("TrackableObject", "Failed to store trackable object");

    private readonly IObjectTypeRepository _objectTypeRepository = objectTypeRepository;
    private readonly ITrackableObjectRepository _trackableObjectRepository = trackableObjectRepository;

    public async Task<Result<TrackableObjectId>> HandleAsync(CreateTrackableObjectCommand request, CancellationToken cancellation)
    {
        var objectTypeId = await _objectTypeRepository.GetIdFromNameAsync(request.ObjectTypeName, cancellation);

        if (!objectTypeId.HasValue)
        {
            return Result<TrackableObjectId>.Failure(InvalidObjectType);
        }

        return await TrackableObject.Factory
            .Create(request.Name, objectTypeId.Value, request.OwnerId)
            .BindAsync(async trackableObject =>
            {
                var storeResult = await _trackableObjectRepository.StoreAsync(trackableObject, cancellation);
                return Result.Success((trackableObject.Id, storeResult));
            })
            .ErrorIfAsync(bind => bind.storeResult == 0, StoringTrackableObjectFailed)
            .MapAsync(bind => bind.Id);
    }
}