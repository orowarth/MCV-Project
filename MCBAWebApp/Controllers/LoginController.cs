using MCBADataLibrary.Data;
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

        HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
        HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
        return RedirectToAction("Index", "Customer");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}