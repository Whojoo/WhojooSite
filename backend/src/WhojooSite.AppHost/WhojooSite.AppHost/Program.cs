var builder = DistributedApplication.CreateBuilder(args);

var apiGateway = builder.AddProject<Projects.WhojooSite_Yarp>("api-gateway")
    .WithExternalHttpEndpoints();

var web = builder.AddNpmApp("web", "../../../../frontend")
    .WithHttpEndpoint(env: "PORT");

var server = builder.AddProject<Projects.WhojooSite_Bootstrap>("server");

var recipeMigrationService = builder.AddProject<Projects.WhojooSite_Recipes_MigrationService>("recipeMigrationService");

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