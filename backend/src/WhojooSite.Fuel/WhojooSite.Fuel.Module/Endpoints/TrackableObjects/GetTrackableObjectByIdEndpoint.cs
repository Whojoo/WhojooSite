using Dapper;

using FastEndpoints;

using WhojooSite.Fuel.Module.Domain.TrackableObjects;
using WhojooSite.Fuel.Module.Infrastructure.Persistence;

namespace WhojooSite.Fuel.Module.Endpoints.TrackableObjects;

internal record GetTrackableObjectByIdRequest(TrackableObjectId Id);

internal record TrackableObjectDto(TrackableObjectId Id, string Name, string ObjectType);

internal class GetTrackableObjectByIdEndpoint(IFuelConnectionFactory fuelConnectionFactory)
    : Endpoint<GetTrackableObjectByIdRequest, TrackableObjectDto>
{
    public override void Configure()
    {
        Get("/trackable-objects/{id:guid}");
        Group<FuelModuleApiGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTrackableObjectByIdRequest req, CancellationToken ct)
    {
        using var connection = await fuelConnectionFactory.CreateConnectionAsync();

        const string sql =
            """
            select trackableObject.id as Id, trackableObject.identifier as Name, objectType.type_name as ObjectType
            from fuel.trackable_object trackableObject
            inner join fuel.object_type objectType on trackableObject.object_type_id = objectType.id
            where trackableObject.id = @Id;
            """;

        var trackableObject = await connection.QueryFirstOrDefaultAsync<TrackableObjectDto?>(sql, new { req.Id });
        if (trackableObject is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(trackableObject, ct);
    }
}