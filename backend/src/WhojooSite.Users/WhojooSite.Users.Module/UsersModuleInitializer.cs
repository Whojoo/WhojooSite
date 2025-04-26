using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

using Serilog;

using WhojooSite.Common.Modules;

namespace WhojooSite.Users.Module;

public class UsersModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Users";

    public void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger)
    {
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
    }
}