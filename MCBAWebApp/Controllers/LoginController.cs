using MCBADataLibrary.Data;
using Microsoft.EntityFrameworkCore;
using MCBADataLibrary.Models;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCBAWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly BankDbContext _context;

        public LoginController(BankDbContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewData)
        {
            var login = await _context.Logins
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(l => l.LoginID == loginViewData.LoginID);

            if (login is null)
            {
                return View();
            }

            // Validate Password using hashing

            // Otherwise...
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}