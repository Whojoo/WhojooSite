using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<WhojooSite_Bootstrap>("server");

var web = builder.AddProject<WhojooSite_View>("web");

var recipeMigrationService =
    builder.AddProject<WhojooSite_Recipes_MigrationService>("recipeMigrationService");

var database = builder
    .AddPostgres("postgres", port: 5432)
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("ServerDb");

recipeMigrationService
    .WithReference(database)
    .WaitFor(database);

web.WithReference(server);

server.WithReference(database);

await builder.Build().RunAsync();