using Microsoft.Kiota.Http.HttpClientLibrary;

namespace WhojooSite.View.Infrastructure.Integrations;

internal static class KiotaServiceCollectionExtensions
{
    public static IServiceCollection AddKiotaHandlers(this IServiceCollection services)
    {
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerActivatableTypes();

        foreach (var handler in kiotaHandlers)
        {
            services.AddTransient(handler);
        }

        return services;
    }

    public static IHttpClientBuilder AttachKiotaHandlers(this IHttpClientBuilder builder)
    {
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerActivatableTypes();

        foreach (var handler in kiotaHandlers)
        {
            builder.AddHttpMessageHandler(sp => (DelegatingHandler)sp.GetRequiredService(handler));
        }

        return builder;
    }
}