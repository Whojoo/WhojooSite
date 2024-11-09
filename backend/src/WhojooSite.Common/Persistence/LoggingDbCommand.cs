using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

namespace WhojooSite.Common.Persistence;

public class LoggingDbCommand(DbCommand command, DbConnection? connection, ILogger logger) : DbCommand
{
    private readonly DbCommand _command = command;
    private readonly ILogger _logger = logger;

    private DbConnection? _connection = connection;

    public override int ExecuteNonQuery()
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return _command.ExecuteNonQuery();
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    public override object? ExecuteScalar()
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return _command.ExecuteScalar();
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return _command.ExecuteReader(behavior);
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    protected override async Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior,
        CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return await _command.ExecuteReaderAsync(behavior, cancellationToken);
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    public override async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return await _command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    public override async Task<object?> ExecuteScalarAsync(CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        try
        {
            return await _command.ExecuteScalarAsync(cancellationToken);
        }
        finally
        {
            LogCommandExecuted(startTimestamp);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _command.Dispose();
        }

        base.Dispose(disposing);
    }

    public override async ValueTask DisposeAsync()
    {
        await _command.DisposeAsync();
        await base.DisposeAsync();
    }

    private void LogCommandExecuted(long startTimestamp)
    {
        _logger.LogInformation("Executed query in {ElapsedMilliseconds} ms:\n{Query}\n",
            Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds,
            CommandText);
    }

    public override void Cancel() => _command.Cancel();

    public override void Prepare() => _command.Prepare();

    [AllowNull]
    public override string CommandText
    {
        get => _command.CommandText;
        set => _command.CommandText = value;
    }

    public override int CommandTimeout
    {
        get => _command.CommandTimeout;
        set => _command.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
        get => _command.CommandType;
        set => _command.CommandType = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
        get => _command.UpdatedRowSource;
        set => _command.UpdatedRowSource = value;
    }

    protected override DbConnection? DbConnection
    {
        get => _connection;
        set => _connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => _command.Parameters;

    protected override DbTransaction? DbTransaction
    {
        get => _command.Transaction;
        set => _command.Transaction = value;
    }

    public override bool DesignTimeVisible
    {
        get => _command.DesignTimeVisible;
        set => _command.DesignTimeVisible = value;
    }

    protected override DbParameter CreateDbParameter()
    {
        return _command.CreateParameter();
    }

    public override async Task PrepareAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await _command.PrepareAsync(cancellationToken);
    }

    public override string ToString() => _command.ToString();

    public override ISite? Site
    {
        get => _command.Site;
        set => _command.Site = value;
    }

    [Obsolete("This Remoting API is not supported and throws PlatformNotSupportedException.",
        DiagnosticId = "SYSLIB0010", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public override object InitializeLifetimeService() => _command.InitializeLifetimeService();

    public override bool Equals(object? obj) => _command.Equals(obj);

    public override int GetHashCode() => _command.GetHashCode();
}