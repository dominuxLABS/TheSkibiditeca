// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;

namespace TheSkibiditeca.Web.Data;

/// <summary>
/// Npgsql-specific DbContext type to support provider-specific migrations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LibraryDbContextNpgsql"/> class.
/// </remarks>
/// <param name="options">The options to be used by the DbContext.</param>
public class LibraryDbContextNpgsql(DbContextOptions<LibraryDbContextSqlServer> options) : LibraryDbContextSqlServer(options)
{
}
