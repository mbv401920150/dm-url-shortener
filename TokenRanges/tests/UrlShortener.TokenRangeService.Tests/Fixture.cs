using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Testcontainers.PostgreSql;

namespace UrlShortener.TokenRangeService.Tests;

public class Fixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    public string ConnectionString => _postgreSqlContainer.GetConnectionString();

    public Fixture()
    {
        _postgreSqlContainer = new PostgreSqlBuilder().Build();
    }
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        Environment.SetEnvironmentVariable("Postgres__ConnectionString", ConnectionString);
        
        await InitializeTokenRangesTable();
    }

    private async Task InitializeTokenRangesTable()
    {
        var tokenRangesTable = await File.ReadAllTextAsync("TokenRangesTable.sql");
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(tokenRangesTable, connection);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}