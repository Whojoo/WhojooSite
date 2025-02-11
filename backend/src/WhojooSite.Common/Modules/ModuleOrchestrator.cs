using System.Diagnostics;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using WhojooSite.Common.Api;

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

    public Assembly[] GetModuleAssemblies() =>
        _moduleInitializers
            .Select(module => module.GetType().Assembly)
            .ToArray();

    public void ConfigureModules(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var moduleInitializer in _moduleInitializers)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            moduleInitializer.ConfigureModule(services, configuration, _logger);
            AddEndpointsForModule(services, moduleInitializer);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            _logger.Information(
                "Module {ModuleName} is configured in {ElapsedMilliseconds} ms",
                moduleInitializer.ModuleName,
                elapsed.TotalMilliseconds);
        }
    }

    public void MapModules(WebApplication app)
    {
        foreach (var moduleInitializer in _moduleInitializers)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            moduleInitializer.MapModule(app, _logger);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);

            _logger.Information(
                "Module {ModuleName} is mapped in {ElapsedMilliseconds} ms",
                moduleInitializer.ModuleName,
                elapsed.TotalMilliseconds);
        }

        var startTime = Stopwatch.GetTimestamp();
        MapEndpoints(app.Services, app);
        var endpointsElapsed = Stopwatch.GetElapsedTime(startTime);
        
        _logger.Information(
            "Mapped endpoints in {ElapsedMilliseconds} ms",
            endpointsElapsed.TotalMilliseconds);
    }

    private static void AddEndpointsForModule(IServiceCollection services, IModuleInitializer moduleInitializer)
    {
        if (!moduleInitializer.HasEndpoints())
        {
            return;
        }

        services.AddEndpointsForAssembly(moduleInitializer.GetType().Assembly);
    }

    private static void MapEndpoints(IServiceProvider serviceProvider, IEndpointRouteBuilder endpointRouteBuilder)
    {
        var mainGroup = endpointRouteBuilder
            .MapGroup("/api")
            .AddEndpointFilter<DomainEventFilter>();
        
        foreach (var endpoint in serviceProvider.GetServices<IEndpoint>())
        {
            endpoint.MapEndpoint(mainGroup);
        }
    }
}