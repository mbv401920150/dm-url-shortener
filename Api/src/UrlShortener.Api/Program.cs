using Azure.Identity;
using UrlShortener.Api.Extensions;
using UrlShortener.Core.Urls.Add;

var builder = WebApplication.CreateBuilder(args);

// This configuration value is added by BICEP script.
// Check the file 'Infrastructure/main.bicep' and 'Infrastructure/Modules/compute/appservice.bicep' 
var keyVaultName = builder.Configuration["KeyVaultName"];

if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"), 
        new DefaultAzureCredential()
    );
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddUrlFeature();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/urls",
    async (AddUrlHandler handler, AddUrlRequest request, CancellationToken cancellationToken) =>
    {
        var requestWithUser = request with { CreatedBy = "mibol@gmail.com" };
        var result = await handler.HandleAsync(requestWithUser, cancellationToken);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created($"/api/urls/{result.Value!.ShortUrl}", result.Value);
    });

app.Run();
