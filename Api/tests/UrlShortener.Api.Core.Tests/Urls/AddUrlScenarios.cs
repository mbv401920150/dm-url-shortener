using Microsoft.Extensions.Time.Testing;
using UrlShortener.Api.Core.Tests.TestDoubles;
using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Tests.Urls;

public class AddUrlScenarios
{
    private readonly AddUrlHandler _handler;
    private readonly InMemoryUrlDataStore _urlDataStore = new();
    private readonly TimeProvider _timeProvider;
    
    public AddUrlScenarios()
    {
        var tokenProvider = new TokenProvider();
        tokenProvider.AssignRange(1, 5);
        
        var shortUrkGenerator = new ShortUrlGenerator(tokenProvider);
        _timeProvider = new FakeTimeProvider();
        _handler = new AddUrlHandler(shortUrkGenerator, _urlDataStore, _timeProvider);
    }
    
    private static AddUrlRequest CreateAddUrlRequest()
    {
        var longUrl = new Uri("https://dometrain.com");

        var request = new AddUrlRequest(longUrl, "admin");
        return request;
    }

    [Fact]
    public async Task Should_return_shortened_url()
    {
        var request = CreateAddUrlRequest();
        
        var response = await _handler.HandleAsync(request, default);

        response.ShortUrl.Should().NotBeEmpty();
        response.ShortUrl.Should().Be("1");
    }

    [Fact]
    public async Task Should_save_short_url()
    {
        var request = CreateAddUrlRequest();
        
        var response = await _handler.HandleAsync(request, default);

        _urlDataStore.Should().ContainKey(response.ShortUrl);
    }

    [Fact]
    public async Task Should_save_short_url_with_created_by_created_on()
    {
        var request = CreateAddUrlRequest();
        
        var response = await _handler.HandleAsync(request, default);

        _urlDataStore.Should().ContainKey(response.ShortUrl);
        _urlDataStore[response.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
        _urlDataStore[response.ShortUrl].CreatedOn.Should().Be(_timeProvider.GetUtcNow());
    }
}

