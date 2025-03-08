using System.Diagnostics;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace WhojooSite.Common.Modules;

public class ModuleOrchestrator(ILogger logger)
{
    private readonly ILogger _logger = logger;

    private readonly List<IModuleInitializer> _moduleInitializers = [];

    public void AddModule<TModule>() where TModule : IModuleInitializer
    {
        var moduleInitializer = Activator.CreateInstance<TModule>();

        var isAlreadyAdded = _moduleInitializers
            .Select(addedModuleInitializer => addedModuleInitializer.ModuleName)
            .Any(name => name == moduleInitializer.ModuleName);

        if (isAlreadyAdded)
        {
            throw new ArgumentException($"Module {moduleInitializer.ModuleName} is already registered");
        }

        _moduleInitializers.Add(moduleInitializer);

        _logger.Information("Module {ModuleName} is registered for configuration", moduleInitializer.ModuleName);
    }

    public void ConfigureModules(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var moduleInitializer in _moduleInitializers)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            moduleInitializer.ConfigureModule(services, configuration, _logger);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            _logger.Information(
                "Module {ModuleName} is configured in {ElapsedMilliseconds} ms",
                moduleInitializer.ModuleName,
                elapsed.TotalMilliseconds);
        }
    }

    public void MapModules(WebApplication app)
    {
        var rootGroup = app.MapGroup("/api");
        
        foreach (var moduleInitializer in _moduleInitializers)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            moduleInitializer.MapEndpoints(rootGroup);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            _logger.Information(
                "Module {ModuleName} is mapped in {ElapsedMilliseconds} ms",
                moduleInitializer.ModuleName,
                elapsed.TotalMilliseconds);
        }
    }
}