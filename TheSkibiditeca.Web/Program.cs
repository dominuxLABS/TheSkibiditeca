// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Identity services using our User entity and integer keys
builder.Services.AddIdentity<TheSkibiditeca.Web.Models.Entities.User, IdentityRole<int>>(options =>
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

// Seed the database and apply migrations (moved to DatabaseInitializer extension)
await app.Services.MigrateAndSeedAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Allow disabling HTTPS redirection when the reverse proxy (Traefik/Coolify) manages TLS.
// Can be controlled via configuration key `DisableHttpsRedirection` or environment
// variables `DISABLE_HTTPS_REDIRECT` / `DISABLE_HTTPS_REDIRECTION` set to "true".
var disableHttps = app.Configuration.GetValue("DisableHttpsRedirection", false);

// Respect common environment variable names as well (useful in containers/Coolify)
var envDisable = Environment.GetEnvironmentVariable("DISABLE_HTTPS_REDIRECT")
                 ?? Environment.GetEnvironmentVariable("DISABLE_HTTPS_REDIRECTION");
if (!string.IsNullOrEmpty(envDisable) && bool.TryParse(envDisable, out var parsed))
{
    disableHttps = parsed;
}

if (!disableHttps)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Lightweight health endpoint used by container orchestrators
app.MapGet("/Health", () => Results.Ok("ok"));

app.Run();
