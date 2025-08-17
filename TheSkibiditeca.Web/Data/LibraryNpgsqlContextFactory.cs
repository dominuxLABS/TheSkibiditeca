// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TheSkibiditeca.Web.Data;

/// <summary>
/// Design-time factory to create the Npgsql DbContext for migrations scaffolding.
/// </summary>
public class LibraryNpgsqlContextFactory : IDesignTimeDbContextFactory<LibraryDbContextNpgsql>
{
    /// <summary>
    /// Creates an instance of <see cref="LibraryDbContextNpgsql"/> for design-time operations such as migrations.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A configured <see cref="LibraryDbContextNpgsql"/>.</returns>
    public LibraryDbContextNpgsql CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContextNpgsql>();

        // Use DATABASE_URL if set; otherwise, use a safe dummy connection string.
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ??
                          "Host=localhost;Port=5432;Database=dummydb;Username=dummy;Password=dummy;SslMode=Prefer";

        optionsBuilder.UseNpgsql(ConnectionStrings.BuildNpgsqlFromUrl(databaseUrl));

        return new LibraryDbContextNpgsql(optionsBuilder.Options);
    }
}
