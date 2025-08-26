// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Logging;

/// <summary>
/// Precompiled logger message delegates used by the application's Program startup.
/// Using <see cref="LoggerMessage"/> improves performance when logging in hot paths.
/// </summary>
public static class ProgramLog
{
    /// <summary>
    /// Logs a warning when a database connection attempt fails.
    /// Parameters: <c>Attempt</c> (int).
    /// </summary>
    public static readonly Action<ILogger, int, Exception?> DbConnectionAttemptFailed =
        LoggerMessage.Define<int>(LogLevel.Warning, new EventId(1001, nameof(DbConnectionAttemptFailed)), "Database connection attempt {Attempt} failed.");

    /// <summary>
    /// Logs an informational message when the database is not ready yet and the app will retry.
    /// Parameters: <c>Attempt</c> (int), <c>MaxAttempts</c> (int), <c>Delay</c> (int milliseconds).
    /// </summary>
    public static readonly Action<ILogger, int, int, int, Exception?> DatabaseNotReady =
        LoggerMessage.Define<int, int, int>(LogLevel.Information, new EventId(1002, nameof(DatabaseNotReady)), "Database not ready yet - attempt {Attempt}/{MaxAttempts}. Waiting {Delay}ms...");
}
