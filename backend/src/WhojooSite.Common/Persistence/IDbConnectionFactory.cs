using System.Data;

namespace WhojooSite.Common.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}