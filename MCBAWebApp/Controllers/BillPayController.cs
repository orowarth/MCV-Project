using MCBADataLibrary.Models;
using MCBADataLibrary.Data;
using MCBAWebApp.Filters;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    public async Task<IActionResult> CancelBill(int id)
    {
        var bill = await _context.BillPayments.FindAsync(id);
        _context.BillPayments.Remove(bill!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> NewBill()
    {
        var customerAccounts = await _context.Accounts
            .Where(a => a.CustomerID == CustomerID)
            .ToListAsync();
        var payees = await _context.Payees.ToListAsync();

        return View(new BillViewModel
        {
            Accounts = customerAccounts.Select(a => new SelectListItem
            {
                Value = a.AccountNumber.ToString(),
                Text = $"{a.AccountNumber}: {a.AccountType}"
            }),
            Payees = payees.Select(p => new SelectListItem
            {
                Value = p.PayeeID.ToString(),
                Text = p.Name
            })
        });
    }

    [HttpPost]
    public async Task<IActionResult> NewBill(BillViewModel viewModel)
    {
        viewModel.Accounts = _context.Accounts
                .Where(a => a.CustomerID == CustomerID)
                .Select(a => new SelectListItem
                {
                    Value = a.AccountNumber.ToString(),
                    Text = $"{a.AccountNumber}: {a.AccountType}"
                });
        viewModel.Payees = _context.Payees.Select(p => new SelectListItem
        {
            Value = p.PayeeID.ToString(),
            Text = p.Name
        });

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        await _context.BillPayments.AddAsync(new BillPay
        {
            AccountNumber = viewModel.SelectedAccount,
            PayeeID = viewModel.SelectedPayee,
            Amount = viewModel.Amount,
            ScheduleTimeUtc = viewModel.ScheduleTime.ToUniversalTime(),
            Period = viewModel.Period
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
