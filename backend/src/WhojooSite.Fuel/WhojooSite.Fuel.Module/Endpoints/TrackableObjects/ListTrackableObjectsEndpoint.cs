using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Fuel.Module.Application.TrackableObjects.List;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Endpoints.TrackableObjects;

internal record ListTrackableObjectsResponse(TrackableObject[] TrackableObjects, int TotalCount);

internal class ListTrackableObjectsEndpoint : IFuelModuleEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/trackable-objects", ListTrackableObjectsAsync);
    }

    private static async Task<Results<Ok<ListTrackableObjectsResponse>, InternalServerError<string>>> ListTrackableObjectsAsync(
        IQueryDispatcher<ListTrackableObjectsQuery, ListTrackableObjectsQueryResponse> queryDispatcher,
        CancellationToken cancellationToken)
    {
        var result = await queryDispatcher.DispatchAsync(new ListTrackableObjectsQuery(OwnerId.Empty), cancellationToken);
        return result.Match<Results<Ok<ListTrackableObjectsResponse>, InternalServerError<string>>>(
            response => TypedResults.Ok(ListTrackableObjectsEndpointMapper.MapToResponse(response)),
            errors => TypedResults.InternalServerError(errors[0].Description));
    }
}

[Mapper]
internal partial class ListTrackableObjectsEndpointMapper
{
    public static partial ListTrackableObjectsResponse MapToResponse(ListTrackableObjectsQueryResponse response);
}