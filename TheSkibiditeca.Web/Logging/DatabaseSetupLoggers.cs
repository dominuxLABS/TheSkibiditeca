// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.Extensions.Logging;

namespace TheSkibiditeca.Web.Logging
{
    /// <summary>
    /// High-performance logger delegates for database setup operations.
    /// Uses LoggerMessage source generators for optimal performance.
    /// </summary>
    public static partial class DatabaseSetupLoggers
    {
        /// <summary>
        /// Log when starting database migrations.
        /// </summary>
        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Information,
            Message = "Applying database migrations...")]
        public static partial void ApplyingMigrations(ILogger logger);

        /// <summary>
        /// Log when database migrations are completed successfully.
        /// </summary>
        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Information,
            Message = "Database migrations applied successfully")]
        public static partial void MigrationsApplied(ILogger logger);

        /// <summary>
        /// Log when starting database seeding.
        /// </summary>
        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Information,
            Message = "Seeding database data...")]
        public static partial void SeedingData(ILogger logger);

        /// <summary>
        /// Log when database seeding is completed successfully.
        /// </summary>
        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Information,
            Message = "Database seeded successfully")]
        public static partial void DataSeeded(ILogger logger);

        /// <summary>
        /// Log when there's an error during database setup.
        /// </summary>
        [LoggerMessage(
            EventId = 1005,
            Level = LogLevel.Error,
            Message = "Error during database setup")]
        public static partial void DatabaseSetupError(ILogger logger, Exception exception);
    }
}
