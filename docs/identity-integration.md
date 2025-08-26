# Integración de ASP.NET Core Identity (Opción B) con cookies y Entity Framework reutilizando la tabla `Users`

Esta guía describe la integración estándar de ASP.NET Core Identity en TheSkibiditeca usando Entity Framework, autenticación por cookies y mapeando la entidad `User` a la tabla existente `Users`. Se añaden únicamente las tablas auxiliares de Identity (roles/claims/tokens), manteniendo las FKs actuales y evitando duplicación de datos.

## Objetivos
- Autenticación por cookies (no JWT).
- Reutilizar la tabla `Users` como fuente única de usuario; mantener FKs (`UserId`) intactas.
- Usar configuración y convenciones lo más cercanas a Identity "out of the box".
- Permitir migraciones (la app no está en producción).

## Integración detallada de ASP.NET Core Identity en TheSkibiditeca (documentación ampliada)

Este documento explica, paso a paso y con ejemplos, cómo se ha integrado ASP.NET Core Identity en TheSkibiditeca reutilizando la tabla existente `Users`, usando cookies para la autenticación y manteniendo las claves foráneas actuales.

Resumen rápido
- Objetivo principal: usar Identity con claves enteras (int) y mapear su modelo sobre la tabla `Users` para no romper relaciones existentes (FKs a `UserId`).
- Autenticación por cookies (cookie name: `skibi.auth`).
- Identity añadirá tablas auxiliares (AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens, AspNetRoleClaims) pero la entidad `User` seguirá usando la tabla `Users`.

Checklist de cambios y artefactos
- [x] `User` ahora hereda de `IdentityUser<int>` y mantiene propiedades de negocio.
- [x] `LibraryDbContext` hereda de `IdentityDbContext<User, IdentityRole<int>, int>` y mapea `User` a `Users`.
- [x] `Program.cs` registra Identity y configura la cookie de aplicación (`skibi.auth`).
- [x] Seeder actualizado para usar `UserManager`/`RoleManager` (ejemplo incluido en la implementación).

1) Paquetes y herramientas
Instala en el proyecto web:

```powershell
dotnet tool install --global dotnet-ef
dotnet add .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj package Microsoft.AspNetCore.Identity.UI
```

2) Cambios en la entidad `User` (contract)
- Entrada/salida: la entidad `User` ahora mantiene los mismos datos de negocio y delega username/phone/password a las propiedades de Identity.
- Forma/contrato: heredar de `IdentityUser<int>`. Mantener atributos de validación sobre las propiedades de negocio.

Ejemplo mínimo recomendado para `Models/Entities/User.cs` (esqueleto):

```csharp
[Table("Users")]
public class User : IdentityUser<int>
{
    [NotMapped]
    public string UserCode { get => this.UserName ?? string.Empty; set => this.UserName = value; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public override string? Email { get; set; }
    public string? Address { get; set; }
    public int UserTypeId { get; set; }
    public string? CareerDepartment { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? MembershipExpirationDate { get; set; }

    // Relaciones
    public virtual UserType UserType { get; set; } = null!;
    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
```

Notas:
- No mantengas `PasswordSalt` — Identity maneja hashing y salt. Si deseas conservar la columna física por compatibilidad, no la mapees desde la clase.

3) Mapeos en `LibraryDbContext`
La idea es heredar de IdentityDbContext y mapear las propiedades de Identity a las columnas existentes:

```csharp
public class LibraryDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.HasKey(u => u.Id);
            b.Property(u => u.Id).HasColumnName("UserId");
            b.Property(u => u.UserName).HasColumnName("UserCode").HasMaxLength(20).IsRequired();
            b.Property(u => u.PhoneNumber).HasColumnName("Phone").HasMaxLength(20);
            b.Property(u => u.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(500);
            b.Property(u => u.Email).HasMaxLength(150);
        });
    }
}
```

Importante:
- No renombres las tablas auxiliares de Identity si quieres poder usar las migraciones y utilidades estándar.

4) Registro de Identity y configuración de cookie (en `Program.cs`)

Registrar Identity (ejemplo):

```csharp
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<LibraryDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.Cookie.Name = "skibi.auth";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.SlidingExpiration = true;
});
```

Orden en la tubería:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

5) Migraciones recomendadas
- Crear migración llamada `IntegrateIdentity` y revisar el SQL generado antes de aplicarla.
- Cambios clave que la migración debe introducir:
  - Agregar columnas Identity a `Users`: NormalizedUserName, NormalizedEmail, EmailConfirmed, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount.
  - Crear tablas AspNet* (roles, claims, logins, tokens, roleclaims, userroles).
  - Mantener `UserId` como columna PK (mapeo via HasColumnName).

Comandos (desde la raíz):

```powershell
dotnet ef migrations add IntegrateIdentity --project .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj --startup-project .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj
dotnet ef database update --project .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj --startup-project .\TheSkibiditeca.Web\TheSkibiditeca.Web.csproj
```

Consejo: revisa el script SQL (`dotnet ef migrations script`) antes de aplicar en entornos con datos.

6) Migración de contraseñas (legacy)
- Si existía un esquema propietario con `PasswordSalt` + hash, necesitas migrar contraseñas para que sean compatibles con Identity.
- Opciones:
  - Forzar a todos los usuarios a resetear contraseña (simplest, cero riesgo): crea un administrador y envía emails para restablecer.
  - Implementar verificación híbrida: conservar la columna legacy, al autenticarse verificar con legacy hash y, si el login es correcto, re-hashear con UserManager and store new PasswordHash (método recomendado si no quieres forzar reseteo inmediato).

Ejemplo de verificación híbrida (alta idea, implementar con cuidado):

1) En Login, buscar usuario por UserCode.
2) Si `PasswordHash` parece del esquema legacy, verificar con el método actual de legacy.
3) Si coincide, llamar `userManager.RemovePasswordAsync(user)` / `userManager.AddPasswordAsync(user, plainPassword)` o usar `userManager.ResetPasswordAsync` con un token para establecer el hash nuevo.

7) Seeder y arranque
- La semilla debe usar `UserManager` y `RoleManager` para crear usuarios y roles — no inserts directos a `Users` para garantizar hashing correcto.
- En `Program.cs` se puede aplicar migraciones y luego llamar a un método async `DbSeeder.SeedDataAsync(serviceProvider, logger)` que resuelve los managers y crea el rol `Admin` y un usuario administrador.

Ejemplo corto de uso en `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await context.Database.MigrateAsync();
    await DbSeeder.SeedDataAsync(scope.ServiceProvider, logger);
}
```

8) Flujo de autenticación y APIs a usar
- Registro: `await UserManager.CreateAsync(user, password)` (asigna `user.UserCode` antes).
- Login: `await SignInManager.PasswordSignInAsync(userCode, password, isPersistent, false)`.
- Logout: `await SignInManager.SignOutAsync()`.

9) Pruebas y verificación
- Pruebas manuales:
  1) Crear rol Admin y usuario administrador.
  2) Hacer login con el administrador y verificar que la cookie `skibi.auth` existe y tiene 8 horas de expiración.
  3) Probar una acción protegida con `[Authorize]`.
  4) Verificar que las tablas `AspNet*` existen y que la tabla `Users` contiene las nuevas columnas.

- Pruebas automáticas sugeridas:
  - Test de integración que levante un proveedor de EF Core en memoria o SQL local, aplique migraciones, ejecute `DbSeeder.SeedDataAsync` y verifique `UserManager.FindByNameAsync` y `SignInManager`.

10) Seguridad y producción
- En producción:
  - Forzar `options.Cookie.SecurePolicy = CookieSecurePolicy.Always`.
  - Revisar SameSite según front-end y SSO.
  - Habilitar bloqueo por intentos (Lockout) si procede.
  - Considerar 2FA y verificación de email.

11) Rollback y plan de contingencia
- Antes de aplicar migraciones en un entorno con datos, exporta un backup y genera script SQL con `dotnet ef migrations script`.
- Si la migración añade columnas no destructivas, la reversión debería ser posible con la migración de rollback; sin embargo si se elimina `PasswordSalt` considera mantenerla sin mapear por un tiempo hasta que todos los usuarios estén migrados.

12) Problemas comunes y soluciones
- "Password incorrecto tras migración": implementar verificación híbrida o forzar reset de contraseñas.
- "FK rompiéndose": comprobar que `Id` esté mapeado a `UserId` y que todos los FKs apunten a `Users(UserId)`.
- "Tablas AspNet* no creadas": ejecutar `dotnet ef database update` y revisar el contexto usado por los comandos.

13) Notas finales y mantenimiento
- Mantén la propiedad puente `UserCode` para minimizar refactors en controladores y vistas; a mediano plazo evalúa refactorizar codebase para usar `UserName` directamente.
- Revisa `DbSeeder` y otros lugares donde se manipulaban contraseñas para garantizar que ahora usan `UserManager`.
