using System.Data;

using Npgsql;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence;

internal class NpgsqlDapperFuelConnectionFactory(string connectionString) : IFuelConnectionFactory
{
    private readonly string _connectionString = connectionString;

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}