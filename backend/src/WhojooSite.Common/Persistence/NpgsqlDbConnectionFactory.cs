using System.Data;

using Microsoft.Extensions.Logging;

using Npgsql;

namespace WhojooSite.Common.Persistence;

public class NpgsqlDbConnectionFactory(ILoggerFactory loggerFactory)
    : IDbConnectionFactory
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public IDbConnection CreateConnection(string connectionString)
    {
        var connectionLogger = _loggerFactory.CreateLogger<LoggingDbConnection>();
        var connection = new NpgsqlConnection(connectionString);

        return connectionLogger.IsEnabled(LogLevel.Information)
            ? new LoggingDbConnection(connection, connectionLogger)
            : connection;
    }
}