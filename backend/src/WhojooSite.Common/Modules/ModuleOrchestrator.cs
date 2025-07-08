using System.Diagnostics;

using Microsoft.Extensions.Hosting;

using Serilog;

namespace WhojooSite.Common.Modules;

public class ModuleOrchestrator(ILogger logger)
{
    private readonly ILogger _logger = logger;
    private readonly List<IModuleInitializer> _moduleInitializers = [];

    public void AddModule<TModule>(TModule moduleInitializer) where TModule : IModuleInitializer
    {
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

    public void ConfigureModules(IHostApplicationBuilder applicationBuilder)
    {
        foreach (var moduleInitializer in _moduleInitializers)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            moduleInitializer.ConfigureModule(applicationBuilder, _logger);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            _logger.Information(
                "Module {ModuleName} is configured in {ElapsedMilliseconds} ms",
                moduleInitializer.ModuleName,
                elapsed.TotalMilliseconds);
        }
    }
}