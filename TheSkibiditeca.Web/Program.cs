// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Logging;
using TheSkibiditeca.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Identity services using our User entity and integer keys
builder.Services.AddIdentity<TheSkibiditeca.Web.Models.Entities.User, Microsoft.AspNetCore.Identity.IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<LibraryDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.Name = "skibi.auth";
    options.Cookie.HttpOnly = true;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
});

// Configure database provider: use SQL Server for both development and production
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
    ?? throw new InvalidOperationException("DefaultConnection not configured");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure(5)));

builder.Services.AddSingleton<ShoppingCart>();

Console.WriteLine("Using SQL Server for database provider");

var app = builder.Build();

// Seed the database and apply migrations
using (var scope = app.Services.CreateScope())
{
    // Resolve the DbContext (SQL Server)
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        DatabaseSetupLoggers.ApplyingMigrations(logger);

        // Wait for the database to be reachable before attempting migrations.
        // This helps in containerized deployments where the DB service may take
        // a few seconds to become ready even after the container starts.
        var maxAttempts = 100; // ~1 minute default (30 * 2000ms)
        var delayMs = 2000;
        var connected = false;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                if (await context.Database.CanConnectAsync())
                {
                    connected = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                // Log and retry using precompiled delegate
                ProgramLog.DbConnectionAttemptFailed(logger, attempt, ex);
            }

            ProgramLog.DatabaseNotReady(logger, attempt, maxAttempts, delayMs, null);
            await Task.Delay(delayMs);
        }

        if (!connected)
        {
            var message = $"Could not connect to the database after {maxAttempts} attempts. Aborting startup.";
            DatabaseSetupLoggers.DatabaseSetupError(logger, new InvalidOperationException(message));
            throw new InvalidOperationException(message);
        }

        await context.Database.MigrateAsync();

        DatabaseSetupLoggers.MigrationsApplied(logger);

        DatabaseSetupLoggers.SeedingData(logger);

        // Call the async Identity-aware seeder which uses UserManager/RoleManager
        await DbSeeder.SeedDataAsync(scope.ServiceProvider, logger);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
