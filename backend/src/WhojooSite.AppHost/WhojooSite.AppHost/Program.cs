var builder = DistributedApplication.CreateBuilder(args);

var apiGateway = builder.AddProject<Projects.WhojooSite_Yarp>("api-gateway")
    .WithExternalHttpEndpoints();

var web = builder.AddNpmApp("web", "../../../../frontend")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

var server = builder.AddProject<Projects.WhojooSite_Bootstrap>("server");

var database = builder.AddPostgres("postgres");
var recipesDb = database.AddDatabase("recipesDb");

apiGateway
    .WithReference(server)
    .WithReference(web);

web.WithReference(apiGateway);

server
    .WithReference(recipesDb)
    .WaitFor(recipesDb);

await builder.Build().RunAsync();