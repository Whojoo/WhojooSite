using System.Data;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence;

internal interface IFuelConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}