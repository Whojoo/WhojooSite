var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres");
var recipesDb = postgres.AddDatabase("recipes-db");

builder
    .AddProject<Projects.WhojooSite_Recipes_MigrationService>("recipes-module-migrations")
    .WithReference(recipesDb);

var server = builder
    .AddProject<Projects.WhojooSite_Bootstrap>("server")
    .WithReference(recipesDb);

var web = builder
    .AddNpmApp("web", "../WhojooSite.Angular")
    .WithHttpEndpoint(env: "PORT");

var yarp = builder
    .AddProject<Projects.WhojooSite_Yarp>("api-gateway")
    .WithReference(server)
    .WithReference(web)
    .WithExternalHttpEndpoints();

web.WithReference(yarp);

builder.Build().Run();