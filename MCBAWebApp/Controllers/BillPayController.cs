using MCBADataLibrary.Models;
using MCBADataLibrary.Data;
using MCBAWebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCBAWebApp.Controllers;

[AuthorizeCustomer]
public class BillPayController : Controller
{
    private readonly BankDbContext _context;
    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID))!.Value;

    public BillPayController(BankDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var bills = await _context.BillPayments
            .Where(b => b.Account.CustomerID == CustomerID)
            .ToListAsync();

        return View(bills);
    }

    public async Task<IActionResult> NewBill() 
    {
        
    }
}
