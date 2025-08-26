// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Logging;

namespace TheSkibiditeca.Web.Data;

/// <summary>
/// Encapsulates database provisioning, migrations and seeding logic.
/// This keeps Program.cs small and focused on wiring services/pipeline.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Ensures the SQL Server is reachable, creates the target database if missing
    /// and applies EF Core migrations, then runs the app data seeder.
    /// </summary>
    /// <param name="services">The application service provider (use <c>app.Services</c>).</param>
    /// <returns>A task that completes when migrations and seeding have finished.</returns>
    public static async Task MigrateAndSeedAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        DatabaseSetupLoggers.ApplyingMigrations(logger);

        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
            ?? throw new InvalidOperationException("DefaultConnection not configured");

        var maxAttempts = 60;
        var delayMs = 2000;
        var serverReady = false;

        try
        {
            var sqlBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var targetDb = sqlBuilder.InitialCatalog;
            if (string.IsNullOrWhiteSpace(targetDb))
            {
                var dbVal = sqlBuilder.ContainsKey("Database") ? (sqlBuilder["Database"] as string) : null;
                targetDb = dbVal ?? string.Empty;
            }

            var adminBuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master",
            };

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using var conn = new Microsoft.Data.SqlClient.SqlConnection(adminBuilder.ConnectionString);
                    await conn.OpenAsync();

                    serverReady = true;

                    if (!string.IsNullOrWhiteSpace(targetDb))
                    {
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = $"IF DB_ID(N'{targetDb}') IS NULL CREATE DATABASE [{targetDb}];";
                        await cmd.ExecuteNonQueryAsync();
                    }

                    break;
                }
                catch (Exception ex)
                {
                    ProgramLog.DbConnectionAttemptFailed(logger, attempt, ex);
                }

                ProgramLog.DatabaseNotReady(logger, attempt, maxAttempts, delayMs, null);
                await Task.Delay(delayMs);
            }
        }
        catch (Exception ex)
        {
            DatabaseSetupLoggers.DatabaseSetupError(logger, ex);
        }

        if (!serverReady)
        {
            var message = $"Could not reach the SQL Server after {maxAttempts} attempts. Aborting startup.";
            DatabaseSetupLoggers.DatabaseSetupError(logger, new InvalidOperationException(message));
            throw new InvalidOperationException(message);
        }

        await context.Database.MigrateAsync();

        DatabaseSetupLoggers.MigrationsApplied(logger);

        DatabaseSetupLoggers.SeedingData(logger);

        await DbSeeder.SeedDataAsync(scope.ServiceProvider, logger);

        DatabaseSetupLoggers.DataSeeded(logger);
    }
}
