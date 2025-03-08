var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults()
    .AddSerilog();

builder.Services
    .AddReverseProxy()
    .AddServiceDiscoveryDestinationResolver()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapReverseProxy();

app.Run();