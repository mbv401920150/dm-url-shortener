using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Tests;

internal class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        Add(shortened.ShortUrl, shortened);
        
        return Task.CompletedTask;
    }
}