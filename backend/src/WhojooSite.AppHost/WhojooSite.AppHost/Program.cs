using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var apiGateway = builder.AddProject<WhojooSite_Yarp>("api-gateway")
    .WithExternalHttpEndpoints();

var web = builder.AddProject<WhojooSite_View>("web");

var server = builder.AddProject<WhojooSite_Bootstrap>("server");

var recipeMigrationService =
    builder.AddProject<WhojooSite_Recipes_MigrationService>("recipeMigrationService");

var database = builder
    .AddPostgres("postgres")
    .WithPgAdmin()
    .AddDatabase("ServerDb");

apiGateway
    .WithReference(server)
    .WithReference(web);

web.WithReference(apiGateway);

recipeMigrationService
    .WithReference(database)
    .WaitFor(database);

server
    .WithReference(database)
    .WaitForCompletion(recipeMigrationService);

await builder.Build().RunAsync();