using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MCBAWebApp.Controllers;

[AuthorizeCustomer]
public class ProfileController : Controller
{
    private readonly BankDbContext _context;

    public ProfileController(BankDbContext context) => _context = context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID))!.Value;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.FindAsync(CustomerID);
        return View(customer);
    }
}