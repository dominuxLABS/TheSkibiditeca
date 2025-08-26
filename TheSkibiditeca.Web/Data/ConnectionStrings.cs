// Copyright (c) dominuxLABS. All rights reserved.

using System.Net;

namespace TheSkibiditeca.Web.Data;

/// <summary>
/// Helper utilities to construct provider-specific connection strings.
/// </summary>
public static class ConnectionStrings
{
    /// <summary>
    /// Builds a safe Npgsql connection string from a postgres:// URL (as provided by Coolify's DATABASE_URL)
    /// or returns the input if it's already a key=value connection string.
    /// </summary>
    /// <param name="urlOrConnString">The postgres:// URL or a key=value connection string.</param>
    /// <returns>A valid Npgsql connection string.</returns>
    public static string BuildNpgsqlFromUrl(string urlOrConnString)
    {
        if (string.IsNullOrWhiteSpace(urlOrConnString))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(urlOrConnString));
        }

        // If it's already a key=value connection string, return as-is
        if (urlOrConnString.StartsWith("Host=", StringComparison.OrdinalIgnoreCase) ||
            urlOrConnString.Contains(';'))
        {
            return urlOrConnString;
        }

        var uri = new Uri(urlOrConnString);

        // Extract credentials (may be URL-encoded)
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = WebUtility.UrlDecode(userInfo[0]);
        var password = userInfo.Length > 1 ? WebUtility.UrlDecode(userInfo[1]) : string.Empty;

        // Construct a simple key=value connection string in a provider-agnostic style.
        // Caller may adapt or replace parts to match their provider (e.g., SqlServer).
        var builder = new System.Text.StringBuilder();
        builder.Append("Host=").Append(uri.Host).Append(';');
        if (uri.Port > 0)
        {
            builder.Append("Port=").Append(uri.Port).Append(';');
        }

        builder.Append("Database=").Append(uri.AbsolutePath.TrimStart('/')).Append(';');
        builder.Append("Username=").Append(username).Append(';');
        builder.Append("Password=").Append(password).Append(';');

        // Parse query parameters (?sslmode=require&timeout=15...)
        var query = uri.Query?.TrimStart('?');
        if (!string.IsNullOrWhiteSpace(query))
        {
            foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                // Skip entries without value (e.g., "?sslmode" with no "=value")
                if (!part.Contains('='))
                {
                    continue;
                }

                var kv = part.Split('=', 2);
                var key = kv[0].Trim().ToLowerInvariant();
                var value = Uri.UnescapeDataString(kv[1].Trim());

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                // Append known or unknown query parameters as key=value pairs
                // so the caller can parse or pass them through to their provider.
                builder.Append(key).Append('=').Append(value).Append(';');
            }
        }

        return builder.ToString();
    }
}
