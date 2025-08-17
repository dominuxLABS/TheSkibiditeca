// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database provider based on environment
if (builder.Environment.IsDevelopment())
{
    // DEVELOPMENT: SQL Server LocalDB
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    builder.Services.AddDbContext<LibraryDbContextSqlServer>(options =>
        options.UseSqlServer(connectionString));

    Console.WriteLine("Using SQL Server LocalDB for development");
}
else
{
    // PRODUCTION: PostgreSQL (URL provided by Coolify resource)
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? throw new InvalidOperationException("DATABASE_URL environment variable not found");

    // Convert postgres:// URI to a safe Npgsql connection string
    var npgsqlConnectionString = ConnectionStrings.BuildNpgsqlFromUrl(databaseUrl);

    builder.Services.AddDbContext<LibraryDbContextNpgsql>(options =>
        options.UseNpgsql(npgsqlConnectionString, npgsql =>
        {
            npgsql.EnableRetryOnFailure(5);
        }));

    Console.WriteLine("Using PostgreSQL from Coolify resource");
}

var app = builder.Build();

// Seed the database and apply migrations
using (var scope = app.Services.CreateScope())
{
    // Resolve the proper DbContext based on environment
    var context = app.Environment.IsDevelopment()
        ? scope.ServiceProvider.GetRequiredService<LibraryDbContextSqlServer>() as DbContext
        : scope.ServiceProvider.GetRequiredService<LibraryDbContextNpgsql>() as DbContext;
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        DatabaseSetupLoggers.ApplyingMigrations(logger);
        await context.Database.MigrateAsync();
        DatabaseSetupLoggers.MigrationsApplied(logger);

        DatabaseSetupLoggers.SeedingData(logger);

    // DbSeeder expects LibraryDbContext; cast when running in dev
        if (context is LibraryDbContextSqlServer lib)
        {
            DbSeeder.SeedData(lib);
        }

        DatabaseSetupLoggers.DataSeeded(logger);
    }
    catch (Exception ex)
    {
        DatabaseSetupLoggers.DatabaseSetupError(logger, ex);
        throw; // Re-throw to prevent app startup with corrupted database
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
