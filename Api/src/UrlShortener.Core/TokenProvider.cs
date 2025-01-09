namespace UrlShortener.Core;

public class TokenProvider
{
    private TokenRange? _tokenRange;
    private long _token = 0;
    private readonly object _tokenLock = new();

    public void AssignRange(long start, long end)
    {
        AssignRange(new TokenRange(start, end));
    }

    public void AssignRange(TokenRange tokenRange)
    {
        _tokenRange = tokenRange;
        _token = tokenRange.Start;
    }

    public long GetToken()
    {
        // Thread safe, only one instance or one accessor could be running at time on this code.
        // Avoiding duplicates.
        lock (_tokenLock)
        {
            return _token++;
        }
    }
}