using Dapper;

using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence.ObjectTypes;

internal class NpgsqlDapperObjectTypesRepository(IFuelConnectionFactory fuelConnectionFactory) : IObjectTypeRepository
{
    private readonly IFuelConnectionFactory _fuelConnectionFactory = fuelConnectionFactory;

    public async Task<ObjectTypeId?> GetIdFromNameAsync(string name, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT id
            FROM fuel.object_type
            WHERE type_name = @TypeName
            LIMIT 1;
            """;

        var connection = await _fuelConnectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<ObjectTypeId>(sql, new { TypeName = name });
    }
}