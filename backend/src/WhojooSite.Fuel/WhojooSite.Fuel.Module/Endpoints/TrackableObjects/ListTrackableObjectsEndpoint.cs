using FastEndpoints;

using Microsoft.AspNetCore.Http;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Fuel.Module.Application.TrackableObjects.List;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Endpoints.TrackableObjects;

internal record ListTrackableObjectsResponse(TrackableObject[] TrackableObjects, int TotalCount);

internal class ListTrackableObjectsEndpoint(
    IQueryDispatcher<ListTrackableObjectsQuery, ListTrackableObjectsQueryResponse> queryDispatcher)
    : EndpointWithoutRequest<ListTrackableObjectsResponse>
{
    public override void Configure()
    {
        Get("/trackable-objects");
        Group<FuelModuleApiGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await queryDispatcher.DispatchAsync(new ListTrackableObjectsQuery(OwnerId.Empty), ct);
        var sendTask = result.Match(
            response => SendOkAsync(ListTrackableObjectsEndpointMapper.MapToResponse(response), ct),
            errors =>
            {
                AddError(errors[0].Description, errors[0].Code);
                return SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
            });
        await sendTask;
    }
}

[Mapper]
internal partial class ListTrackableObjectsEndpointMapper
{
    public static partial ListTrackableObjectsResponse MapToResponse(ListTrackableObjectsQueryResponse response);
}