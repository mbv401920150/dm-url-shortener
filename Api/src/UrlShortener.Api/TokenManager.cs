using UrlShortener.Core;

public class TokenManager : IHostedService
{
    private readonly ITokenRangeApiClient _client;
    private readonly ILogger<TokenManager> _logger;
    private readonly TokenProvider _tokenProvider;
    private readonly IEnvironmentManager _environmentManager;
    private readonly string _machineIdentifier;

    public TokenManager(ITokenRangeApiClient client, ILogger<TokenManager> logger, TokenProvider tokenProvider, IEnvironmentManager environmentManager)
    {
        _client = client;
        _logger = logger;
        _tokenProvider = tokenProvider;
        _environmentManager = environmentManager;
        _machineIdentifier = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? "unknown";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Ask for a new range
            _logger.LogInformation("TokenManager is starting.");

            _tokenProvider.ReachingRangeLimit += async (sender, args) =>
            {
                await AssignNewTokenRangeAsync(cancellationToken);    
            };

            await AssignNewTokenRangeAsync(cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical("TokenManager failed to start due to an error.");
            // Stop the application with a fatal error
            _environmentManager.FatalError();
        }
    }

    private async Task AssignNewTokenRangeAsync(CancellationToken cancellationToken)
    {
        var range = await _client.AssignRangeAsync(_machineIdentifier, cancellationToken);

        if (range is null)
        {
            throw new Exception("No token assigned.");
        }

        _tokenProvider.AssignRange(range);

        _logger.LogInformation("Assigned range: {Start} - {End}.", range.Start, range.End);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TokenManager is stopping.");
        return Task.CompletedTask;
    }
}