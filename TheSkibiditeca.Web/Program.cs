// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database provider: use SQL Server for both development and production
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
    ?? throw new InvalidOperationException("DefaultConnection not configured");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(5)));

Console.WriteLine("Using SQL Server for database provider");

var app = builder.Build();

// Seed the database and apply migrations
using (var scope = app.Services.CreateScope())
{
    // Resolve the DbContext (SQL Server)
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>() as DbContext;
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        DatabaseSetupLoggers.ApplyingMigrations(logger);
        await context.Database.MigrateAsync();
        DatabaseSetupLoggers.MigrationsApplied(logger);

        DatabaseSetupLoggers.SeedingData(logger);

    // DbSeeder expects LibraryDbContext; cast when running in dev
        if (context is LibraryDbContext lib)
        {
            DbSeeder.SeedData(lib, logger);
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
