using Dapper;

using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.FuelEntries;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence.FuelEntries;

internal class NpgsqlDapperFuelEntriesRepository(IFuelConnectionFactory connectionFactory) : IFuelEntryRepository
{
    private readonly IFuelConnectionFactory _connectionFactory = connectionFactory;

    public async Task<FuelEntry?> GetAsync(FuelEntryId id, CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT id, mileage, creation_date, execution_date, refuel_amount, fuel_liter_price, fuel_total_price, object_id
            FROM fuel.fuel_entry
            WHERE id = @Id;
            """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryFirstOrDefaultAsync<FuelEntry>(sql, new { Id = id });
    }

    public async Task<int> StoreAsync(FuelEntry fuelEntry, CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO fuel.fuel_entry(id, mileage, creation_date, execution_date, refuel_amount, fuel_liter_price, 
                                        fuel_total_price, object_id) 
            VALUES (@Id, @Mileage, @CreationDate, @ExecutionDate, @RefuelAmount, @LiterPrice, @TotalCost, @ObjectId);
            """;

        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.ExecuteAsync(sql, fuelEntry);
    }
}