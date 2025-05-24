using Riok.Mapperly.Abstractions;

using WhojooSite.Common.Handlers;
using WhojooSite.Common.Results;
using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.Shared;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;
using WhojooSite.Fuel.Module.Shared.Dtos;

namespace WhojooSite.Fuel.Module.Application.TrackableObjects.List;

internal record ListTrackableObjectsQuery(OwnerId OwnerId) : IQuery<ListTrackableObjectsQueryResponse>;

internal record ListTrackableObjectsQueryResponse(TrackableObject[] TrackableObjects, int TotalCount);

internal class ListTrackableObjectsHandler(ITrackableObjectRepository trackableObjectRepository)
    : IQueryHandler<ListTrackableObjectsQuery, ListTrackableObjectsQueryResponse>
{
    private readonly ITrackableObjectRepository _trackableObjectRepository = trackableObjectRepository;

    public async Task<Result<ListTrackableObjectsQueryResponse>> HandleAsync(ListTrackableObjectsQuery request,
        CancellationToken cancellation)
    {
        var resultList = await _trackableObjectRepository.ListAsync(cancellation);
        return Result<ListTrackableObjectsQueryResponse>.Success(ListTrackableObjectsHandlerMapper.MapToResponse(resultList));
    }
}

[Mapper]
internal static partial class ListTrackableObjectsHandlerMapper
{
    public static partial ListTrackableObjectsQueryResponse MapToResponse(ListTrackableObjectsResultDto dto);
}