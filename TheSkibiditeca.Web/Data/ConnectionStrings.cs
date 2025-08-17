// Copyright (c) dominuxLABS. All rights reserved.

using System.Net;
using Npgsql;

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

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = username,
            Password = password,

            // Default when URL has dangling ?sslmode without value
            SslMode = SslMode.Prefer,
        };

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

                switch (key)
                {
                    case "sslmode":
                        if (Enum.TryParse<SslMode>(value, ignoreCase: true, out var mode))
                        {
                            builder.SslMode = mode;
                        }

                        break;

                    case "timeout":
                        if (int.TryParse(value, out var timeout))
                        {
                            builder.Timeout = timeout;
                        }

                        break;

                    case "commandtimeout":
                        if (int.TryParse(value, out var cmdTimeout))
                        {
                            builder.CommandTimeout = cmdTimeout;
                        }

                        break;

                    case "pooling":
                        if (bool.TryParse(value, out var pooling))
                        {
                            builder.Pooling = pooling;
                        }

                        break;

                    // ignore unknown keys to avoid KeyNotFoundException
                    default:
                        break;
                }
            }
        }

        return builder.ConnectionString;
    }
}
