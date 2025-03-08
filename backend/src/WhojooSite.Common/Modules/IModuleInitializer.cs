using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace WhojooSite.Common.Modules;

public interface IModuleInitializer
{
    string ModuleName { get; }

    void ConfigureModule(IServiceCollection services, IConfiguration configuration, ILogger logger);
    void MapEndpoints(RouteGroupBuilder routeGroupBuilder);
}