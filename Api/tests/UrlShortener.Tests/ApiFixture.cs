using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.Api;

namespace UrlShortener.Tests;

/// <summary>
/// The WebApplicationFactory requires the package Microsoft.AspNetCore.Mvc.Testing
/// Instead to use Program.cs of the REST API, a good approach could be to add an Interface named 'IAssemblyMarker`
/// </summary>
public class ApiFixture : WebApplicationFactory<IAssemblyMarker>
{
    
}