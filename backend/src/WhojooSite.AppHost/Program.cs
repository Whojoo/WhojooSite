var builder = DistributedApplication.CreateBuilder(args);

var server = builder
    .AddProject<Projects.WhojooSite_Bootstrap>("server");
    // .WithExternalHttpEndpoints();

var yarp = builder
    .AddProject<Projects.WhojooSite_Yarp>("api-gateway")
    .WithReference(server)
    .WithExternalHttpEndpoints();

var web = builder
    .AddNpmApp("web", "../WhojooSite.Angular")
    .WithReference(yarp)
    .WithHttpEndpoint(env: "PORT");

yarp = yarp.WithReference(web);

builder.Build().Run();