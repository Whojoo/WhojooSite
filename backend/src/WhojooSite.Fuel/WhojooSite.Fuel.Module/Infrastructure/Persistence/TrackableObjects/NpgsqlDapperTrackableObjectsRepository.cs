using Dapper;

using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;
using WhojooSite.Fuel.Module.Shared.Dtos;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence.TrackableObjects;

internal class NpgsqlDapperTrackableObjectsRepository(IFuelConnectionFactory fuelConnectionFactory) : ITrackableObjectRepository
{
    private readonly IFuelConnectionFactory _fuelConnectionFactory = fuelConnectionFactory;

    public async Task<TrackableObject?> GetAsync(TrackableObjectId id, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT id, identifier, object_type_id, owner_id, creation_date
            FROM fuel.trackable_object
            WHERE id = @Id;
            """;

        using var connection = await _fuelConnectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<TrackableObject>(sql, new { Id = id });
    }

    public async Task<int> StoreAsync(TrackableObject trackableObject, CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO fuel.trackable_object(id, identifier, object_type_id, owner_id, creation_date) 
            VALUES (@Id, @Name, @ObjectTypeId, @OwnerId, @CreationDate);
            """;

        using var connection = await _fuelConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(sql, trackableObject);
    }

    public async Task<ListTrackableObjectsResultDto> ListAsync(CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT trackable.id, trackable.identifier, trackable.object_type_id, trackable.owner_id, trackable.creation_date, 
                   type.id, type.type_name, type.creation_date
            FROM fuel.trackable_object AS trackable
            INNER JOIN fuel.object_type type ON type.id = object_type_id;

            SELECT COUNT(*) FROM fuel.trackable_object;
            """;

        using var connection = await _fuelConnectionFactory.CreateConnectionAsync();

        var multiRead = await connection.QueryMultipleAsync(sql);

        var trackableObjects = multiRead.Read<TrackableObject, ObjectType, TrackableObject>((to, ot) =>
        {
            to.ObjectType = ot;
            return to;
        }).ToArray();

        var totalCount = multiRead.Read<int>().First();

        return new ListTrackableObjectsResultDto(trackableObjects, totalCount);
    }
}