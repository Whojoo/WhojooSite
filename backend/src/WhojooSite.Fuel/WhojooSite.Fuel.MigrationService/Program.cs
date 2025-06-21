// See https://aka.ms/new-console-template for more information

using System.Reflection;

using DbUp;

// Set the environment in the run configuration
var connectionString = Environment.GetCommandLineArgs()[1];

EnsureDatabase.For.PostgresqlDatabase(connectionString);

var upgrader =
    DeployChanges
        .To
        .PostgresqlDatabase(connectionString)
        .JournalToPostgresqlTable("fuel", "schema_versions")
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .WithTransaction()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();

    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();

return 0;