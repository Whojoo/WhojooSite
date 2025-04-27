using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace WhojooSite.Common.Modules;

public interface IModuleInitializer
{
    string ModuleName { get; }

    void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger);
    void MapEndpoints(RouteGroupBuilder routeGroupBuilder);
    void MapModule(WebApplication app);
}