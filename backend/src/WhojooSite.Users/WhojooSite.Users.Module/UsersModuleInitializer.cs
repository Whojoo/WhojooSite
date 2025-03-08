using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using WhojooSite.Common.Modules;

namespace WhojooSite.Users.Module;

public class UsersModuleInitializer : IModuleInitializer
{
    public string ModuleName => "Users";

    public void ConfigureModule(IServiceCollection services, IConfiguration configuration, ILogger logger)
    {
    }

    public void MapEndpoints(RouteGroupBuilder routeGroupBuilder)
    {
    }
}