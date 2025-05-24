using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common.Modules;
using WhojooSite.Fuel.Module.Application;
using WhojooSite.Fuel.Module.Endpoints;
using WhojooSite.Fuel.Module.Infrastructure;

namespace WhojooSite.Fuel.Module;

public class FuelModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Fuel";

    public void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger)
    {
        applicationBuilder.Services
            .AddInfrastructure()
            .AddApplication()
            .AddEndpoints();
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
        var fuelModuleGroup = routeGroupBuilder
            .MapGroup("/fuel-module")
            .WithGroupName("fuel-module")
            .WithOpenApi();

        fuelModuleGroup.MapEndpoints();
    }

    public void MapModule(WebApplication app)
    {
    }
}