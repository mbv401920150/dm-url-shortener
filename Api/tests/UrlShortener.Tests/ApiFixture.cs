using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Api;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Tests.Extensions;
using UrlShortener.Tests.TestDoubles;

namespace UrlShortener.Tests;

/// <summary>
/// The WebApplicationFactory requires the package Microsoft.AspNetCore.Mvc.Testing
/// Instead to use Program.cs of the REST API, a good approach could be to add an Interface named 'IAssemblyMarker`
/// </summary>
public class ApiFixture : WebApplicationFactory<IAssemblyMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                // Replace the Azure Implementation and use InMemory Data Source
                services.Remove<IUrlDataStore>();
                services.AddSingleton<IUrlDataStore>(new InMemoryUrlDataStore());
                
                services.Remove<ITokenRangeApiClient>();
                services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeClient>();
            });
        
        base.ConfigureWebHost(builder);
    }
}