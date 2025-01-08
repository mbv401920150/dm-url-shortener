using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    public static void Remove<T>(this IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => ReferenceEquals(d.ServiceKey, typeof(T)));
        if (descriptor != null) services.Remove(descriptor);
    }
}