using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;

namespace MCBAWebApp.Controllers;

public class LoginController : Controller
{
    private readonly BankDbContext _context;
    private static readonly SimpleHash simpleHash = new();

    public LoginController(BankDbContext context) => _context = context;

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewData)
    {
        var login = await _context.Logins
            .Include(l => l.Customer)
            .ThenInclude(c => c.Image)
            .FirstOrDefaultAsync(l => l.LoginID == loginViewData.LoginID);

        if (!ModelState.IsValid)
        {
            return View();
        }

        if (login is null || !simpleHash.Verify(loginViewData.Password, login.PasswordHash))
        {
            ModelState.AddModelError("LoginFailure", "Login ID and/or password were incorrect.");
            return View();
        }

        if (login.Customer.CustomerStatus == CustomerStatus.Blocked)
        {
            ModelState.AddModelError("LoginFailure", "Your account has been blocked, please contact MCBA administrators.");
            return View();
        }

        HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
        HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);

        if (login.Customer.Image is not null && login.Customer.Image.Image is not null)
        {
            HttpContext.Session.SetString(nameof(Customer.Image), login.Customer.Image.Image);
        }

        return RedirectToAction("Index", "Customer");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}