var builder = DistributedApplication.CreateBuilder(args);

var server = builder
    .AddProject<Projects.WhojooSite_Bootstrap>("server");
    // .WithExternalHttpEndpoints();

var yarp = builder
    .AddProject<Projects.WhojooSite_Yarp>("api-gateway")
    .WithReference(server)
    .WithExternalHttpEndpoints();

builder.Build().Run();