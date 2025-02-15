using Microsoft.AspNetCore.Builder;
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

    public void MapModule(WebApplication app, ILogger logger)
    {
    }

    public bool HasEndpoints() => true;
}