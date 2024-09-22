using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var recipesDb = postgres.AddDatabase("recipes-db");

builder
    .AddProject<WhojooSite_Recipes_MigrationService>("recipes-module-migrations")
    .WithReference(recipesDb);

var server = builder
    .AddProject<WhojooSite_Bootstrap>("server")
    .WithReference(recipesDb);

var web = builder
    .AddNpmApp("web", "../WhojooSite.Angular")
    .WithHttpEndpoint(env: "PORT");

var yarp = builder
    .AddProject<WhojooSite_Yarp>("api-gateway")
    .WithReference(server)
    .WithReference(web)
    .WithExternalHttpEndpoints();

web.WithReference(yarp);

builder.Build().Run();