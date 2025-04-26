using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .ConfigureDefaultSerilog(builder)
    .CreateLogger();

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