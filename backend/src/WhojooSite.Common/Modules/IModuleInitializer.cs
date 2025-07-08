using Microsoft.Extensions.Hosting;

using Serilog;

namespace WhojooSite.Common.Modules;

public interface IModuleInitializer
{
    string ModuleName { get; }

    void ConfigureModule(IHostApplicationBuilder applicationBuilder, ILogger logger);
}