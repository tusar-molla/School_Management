using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using School_Management.Data;
using School_Management.Helpers;
using School_Management.Models;

namespace School_Management.Controllers
{
    public class AuthController : Controller
    {
        private readonly SchoolDbContext _context;

        public AuthController(SchoolDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid || model.Password != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Registration failed. Please try again.";
                return RedirectToAction("Register");
            }

            var existingUser = await _context.users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Username already exists.";
                return RedirectToAction("Register");
            }

            // Hash the password before saving it
            var user = new User
            {
                Username = model.Username,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                RoleId = model.RoleId
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Auth");                     
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid Login Details.";
                return RedirectToAction("Login");
            }
            var user = await _context.users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null || user.PasswordHash != PasswordHelper.HashPassword(model.Password))
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToAction("Login");
            }
            // Create the user claims for authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.RoleName)  // Sets role for authorization
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,  // Keeps user logged in until they log out
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }
    }
}
