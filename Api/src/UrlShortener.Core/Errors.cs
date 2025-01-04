namespace UrlShortener.Core;

public static class Errors
{
    public static Error MissingCreatedBy => new Error("missing_data", "Created by is required");
}