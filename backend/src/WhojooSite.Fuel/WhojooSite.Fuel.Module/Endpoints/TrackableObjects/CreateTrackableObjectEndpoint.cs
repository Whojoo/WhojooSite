using FastEndpoints;

using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Fuel.Module.Application.TrackableObjects.Create;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Endpoints.TrackableObjects;

internal record CreateTrackableObjectRequest(string Name, OwnerId OwnerId, string ObjectTypeName);

internal class CreateTrackableObjectEndpoint(
    ICommandDispatcher<CreateTrackableObjectCommand, TrackableObjectId> commandDispatcher)
    : Endpoint<CreateTrackableObjectRequest>
{
    public override void Configure()
    {
        Post("/trackable-objects");
        Group<FuelModuleApiGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTrackableObjectRequest req, CancellationToken ct)
    {
        var command = CreateTrackableObjectEndpointMapper.MapToCommand(req);
        var result = await commandDispatcher.DispatchAsync(command, ct);

        var sendTask = result.Match(
            _ => SendAsync(null, StatusCodes.Status201Created, ct),
            errors =>
            {
                foreach (var error in errors)
                {
                    AddError(new ValidationFailure(error.Code, error.Description));
                }

                return SendErrorsAsync(cancellation: ct);
            });

        await sendTask;
    }
}

[Mapper]
internal static partial class CreateTrackableObjectEndpointMapper
{
    public static partial CreateTrackableObjectCommand MapToCommand(CreateTrackableObjectRequest request);
}