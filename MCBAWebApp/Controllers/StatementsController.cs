using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Filters;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace MCBAWebApp.Controllers;

[AuthorizeCustomer]
public class StatementsController : Controller
{
    private readonly BankDbContext _context;

    public StatementsController(BankDbContext context) => _context = context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID))!.Value;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers
            .Include(x => x.Accounts)
            .FirstAsync(c => c.CustomerID == CustomerID);

        return View(customer);
    }

    public async Task<IActionResult> Statement(int id, int? page)
    {
        var account = await _context.Accounts
            .Include(a => a.Transactions)
            .FirstAsync(a => a.AccountNumber == id);

        int pageSize = 4;
        int pageNumber = page ?? 1;

        return View(new StatementsViewModel() 
        { 
            Account = account, 
            Transactions = account.Transactions
                .OrderByDescending(t => t.TransactionTimeUtc)
                .ToPagedList(pageNumber, pageSize) 
        });
    }
}