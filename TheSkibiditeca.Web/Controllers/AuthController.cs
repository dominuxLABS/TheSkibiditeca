// Copyright (c) dominuxLABS. All rights reserved.

using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheSkibiditeca.Web.Data;
using TheSkibiditeca.Web.Models.Auth;
using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Controllers
{
    /// <summary>
    /// Handles authentication-related pages (register, login) and actions.
    /// </summary>
    public class AuthController : Controller
    {
        private readonly LibraryDbContext db;
        private readonly UserManager<User> userM;
        private readonly SignInManager<User> singIn;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="context">The library database context.</param>
        /// <param name="userM">The user manager used to create and manage application users.</param>
        /// <param name="singIn">The sign-in manager used to handle user sign-in and sign-out operations.</param>
        public AuthController(LibraryDbContext context, UserManager<User> userM, SignInManager<User> singIn)
        {
            this.db = context;
            this.userM = userM;
            this.singIn = singIn;
        }

        /// <summary>
        /// Shows the default auth index page.
        /// </summary>
        /// <returns>The auth index view.</returns>
        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET: registration form.
        /// </summary>
        /// <summary>
        /// Displays the user registration page.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// POST: handle registration form submission (placeholder).
        /// Currently returns the view; implement registration with UserManager in the Identity integration.
        /// </summary>
        /// <summary>
        /// Processes the registration form submission (placeholder).
        /// </summary>
        /// <param name="model">The registration model submitted by the user.</param>
        /// <returns>The registration view with validation state.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model.passwordConf != model.passwordStr)
            {
                return this.View(model);
            }

            if (model != null)
            {
                var user = new User()
                {
                    FirstName = model.user.FirstName,
                    LastName = model.user.LastName,
                    Email = model.user.Email,
                    UserName = model.user.Email,
                    Phone = model.user.PhoneNumber,
                    UserCode = GenerateUserCode(model.user),
                    UserTypeId = 1,
                };
                var result = await this.userM.CreateAsync(user, model.passwordStr);
                if (result.Succeeded)
                {
                    Console.WriteLine("hola");
                    await this.singIn.SignInAsync(user, isPersistent: false);
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        // Log or display the error.Code and error.Description
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Modlo xd");
            }

            await Task.CompletedTask;
            return this.View(model);
        }

        /// <summary>
        /// Shows the login page.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            this.ViewBag.Falied = false;
            return this.View();
        }

        /// <summary>
        /// Processes the login form submission.
        /// </summary>
        /// <param name="model">The login model containing the user's email and password.</param>
        /// <returns>Redirects to the home index on successful sign-in; otherwise returns the login view with failure state.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            this.ViewBag.Falied = false;

            if (model == null || string.IsNullOrEmpty(model.email))
            {
                this.ViewBag.Falied = true;
                return this.View();
            }

            User? signedUser = await this.userM.FindByEmailAsync(model.email);
            if (signedUser != null)
            {
                var result = await this.singIn.CheckPasswordSignInAsync(signedUser, model.password, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ViewBag.Falied = true;
                    return this.View();
                }
            }
            else
            {
                this.ViewBag.Falied = true;
                return this.View();
            }
        }

        /// <summary>
        /// Signs the current user out and redirects to the home index page.
        /// </summary>
        /// <returns>A redirect to the Home controller's Index action.</returns>
        public async Task<IActionResult> Logout()
        {
            await this.singIn.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        private static string GenerateUserCode(User user)
        {
            string initials = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                ? $"{user.FirstName[0]}{user.LastName[0]}".ToUpper(CultureInfo.InvariantCulture)
                : "ES"; // Default si no hay nombre/apellido

            string timestamp = DateTime.Now.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture); // Año (2), mes, día, hora, minuto, segundo
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper(CultureInfo.InvariantCulture); // 4 caracteres aleatorios
            return $"ST{initials}{timestamp}{randomPart}";
        }
    }
}
