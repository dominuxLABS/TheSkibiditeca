// Copyright (c) dominuxLABS. All rights reserved.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
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
        private readonly UserManager<User> _userM;
        private readonly SignInManager<User> _singIn;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="context">The library database context.</param>
        public AuthController(LibraryDbContext context, UserManager<User> userM, SignInManager<User> singIn)
        {
            this.db = context;
            _userM = userM;
            _singIn = singIn;
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
        public async Task<IActionResult> Register(RegisterModel model) {
            if (model.passwordConf != model.passwordStr) {
                return this.View(model);
            }
            if (model != null) {
                var user = new User() {
                    FirstName = model.user.FirstName,
                    LastName = model.user.LastName,
                    Email = model.user.Email,
                    UserName = model.user.Email,
                    Phone = model.user.PhoneNumber,
                    UserCode = GenerateUserCode(model.user),
                    UserTypeId = 1,
                };
                var result = await _userM.CreateAsync(user, model.passwordStr);
                if(result.Succeeded) {
                    Console.WriteLine("hola");
                    await _singIn.SignInAsync(user, isPersistent: false);
                    return this.RedirectToAction("Index", "Home");
                } else {
                    foreach(var error in result.Errors) {
                        // Log or display the error.Code and error.Description
                        Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    }
                }
            }else {
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
            ViewBag.Falied = false;
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model) {
            ViewBag.Falied = false;
            User signedUser = await _userM.FindByEmailAsync(model.email);
            var result = await _singIn.PasswordSignInAsync(signedUser.UserName, model.password, false, lockoutOnFailure: true);
            if(result.Succeeded) {
                return RedirectToAction("Index", "Home");
            }else {
                ViewBag.Falied = true;
                return this.View();
            }    
        }

        public async Task<IActionResult> Logout() {
            await _singIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private string GenerateUserCode(User user) {
            string initials = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                ? $"{user.FirstName[0]}{user.LastName[0]}".ToUpper()
                : "US"; // Default si no hay nombre/apellido

            string timestamp = DateTime.Now.ToString("yyMMddHHmmss"); // Año (2), mes, día, hora, minuto, segundo
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper(); // 4 caracteres aleatorios
            return $"ST{initials}{timestamp}{randomPart}";
        }
    }
}
