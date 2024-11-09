using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

using Microsoft.Extensions.Logging;

using IsolationLevel = System.Data.IsolationLevel;

namespace WhojooSite.Common.Persistence;

public class LoggingDbConnection(
    DbConnection connection,
    ILogger<LoggingDbConnection> logger)
    : DbConnection
{
    private readonly DbConnection _connection = connection;
    private readonly ILogger<LoggingDbConnection> _logger = logger;

    public override void Close()
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            _connection.Close();
        }
        finally
        {
            LogConnectionClosed(startTimestamp, _logger);
        }
    }

    public override async Task CloseAsync()
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            await _connection.CloseAsync();
        }
        finally
        {
            LogConnectionClosed(startTimestamp, _logger);
        }
    }

    public override void Open()
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            _connection.Open();
        }
        finally
        {
            LogConnectionOpened(startTimestamp, _logger);
        }
    }

    public override async Task OpenAsync(CancellationToken cancellationToken)
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            await _connection.OpenAsync(cancellationToken);
        }
        finally
        {
            LogConnectionOpened(startTimestamp, _logger);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection.Dispose();
        }

        base.Dispose(disposing);
    }

    public override async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override DbCommand CreateDbCommand()
    {
        var command = _connection.CreateCommand();
        return new LoggingDbCommand(command, this, _logger);
    }

    private static void LogConnectionClosed(long startTimestamp, ILogger<LoggingDbConnection> logger)
    {
        logger.LogInformation(
            "Connection to database was closed in {ElapsedMilliseconds} ms",
            Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds);
    }

    private static void LogConnectionOpened(long startTimestamp, ILogger<LoggingDbConnection> logger)
    {
        logger.LogInformation(
            "Connection to database was opened in {ElapsedMilliseconds} ms",
            Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds);
    }

    // Other public member, no need to wrap those

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) =>
        _connection.BeginTransaction(isolationLevel);

    [AllowNull]
    public override string ConnectionString
    {
        get => _connection.ConnectionString;
        set => _connection.ConnectionString = value;
    }

    public override string Database => _connection.Database;
    public override ConnectionState State => _connection.State;
    public override string DataSource => _connection.DataSource;
    public override string ServerVersion => _connection.ServerVersion;
    public override void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);

    public override Task ChangeDatabaseAsync(string databaseName,
        CancellationToken cancellationToken = new CancellationToken())
        => _connection.ChangeDatabaseAsync(databaseName, cancellationToken);

    public override void EnlistTransaction(Transaction? transaction) => _connection.EnlistTransaction(transaction);
    public override DataTable GetSchema() => _connection.GetSchema();
    public override DataTable GetSchema(string collectionName) => _connection.GetSchema(collectionName);

    public override DataTable GetSchema(string collectionName, string?[] restrictionValues)
        => _connection.GetSchema(collectionName, restrictionValues);

    public override Task<DataTable> GetSchemaAsync(CancellationToken cancellationToken = new CancellationToken())
        => _connection.GetSchemaAsync(cancellationToken);

    public override Task<DataTable> GetSchemaAsync(string collectionName,
        CancellationToken cancellationToken = new CancellationToken())
        => _connection.GetSchemaAsync(collectionName, cancellationToken);

    public override Task<DataTable> GetSchemaAsync(string collectionName, string?[] restrictionValues,
        CancellationToken cancellationToken = new CancellationToken())
        => _connection.GetSchemaAsync(collectionName, restrictionValues, cancellationToken);

    public override int ConnectionTimeout => _connection.ConnectionTimeout;
    public override bool CanCreateBatch => _connection.CanCreateBatch;

    public override event StateChangeEventHandler? StateChange
    {
        add => _connection.StateChange += value;
        remove => _connection.StateChange -= value;
    }

    public override string ToString() => _connection.ToString();
    public override ISite? Site { get; set; }
    public override bool Equals(object? obj) => _connection.Equals(obj);
    public override int GetHashCode() => _connection.GetHashCode();

    protected override ValueTask<DbTransaction> BeginDbTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken)
        => _connection.BeginTransactionAsync(isolationLevel, cancellationToken);

    protected override DbBatch CreateDbBatch() => _connection.CreateBatch();

    [Obsolete("This Remoting API is not supported and throws PlatformNotSupportedException.",
        DiagnosticId = "SYSLIB0010", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public override object InitializeLifetimeService() => _connection.InitializeLifetimeService();
}