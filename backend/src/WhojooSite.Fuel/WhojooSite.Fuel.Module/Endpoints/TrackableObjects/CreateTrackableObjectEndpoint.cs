using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Application.TrackableObjects.Create;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Endpoints.TrackableObjects;

internal record CreateTrackableObjectRequest(string Name, OwnerId OwnerId, string ObjectTypeName);

internal class CreateTrackableObjectEndpoint : IFuelModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/trackable-objects", CreateTrackableObjectAsync);
    }

    private static async Task<Results<Created, ValidationProblem>> CreateTrackableObjectAsync(
        CreateTrackableObjectRequest request,
        ICommandDispatcher<CreateTrackableObjectCommand, TrackableObjectId> commandDispatcher,
        CancellationToken cancellationToken)
    {
        var command = CreateTrackableObjectEndpointMapper.MapToCommand(request);
        var result = await commandDispatcher.DispatchAsync(command, cancellationToken);

        return result.Match<Results<Created, ValidationProblem>>(
            _ => TypedResults.Created(),
            errors => errors.MapToValidationProblem());
    }
}

[Mapper]
internal static partial class CreateTrackableObjectEndpointMapper
{
    public static partial CreateTrackableObjectCommand MapToCommand(CreateTrackableObjectRequest request);
}