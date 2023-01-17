using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCBAWebApp.Controllers;

public class CustomerController : Controller
{
    private readonly BankDbContext _context;
    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID))!.Value;

    public CustomerController(BankDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers
            .Include(x => x.Accounts)
            .FirstAsync(c => c.CustomerID == CustomerID);

        return View(customer);
    }

    public async Task<IActionResult> Deposit(int id)
    {
        return View(new DepositViewModel()
        {
            Account = await _context.Accounts.FirstAsync(a => a.AccountNumber == id),
            AccountNumber = id
        });
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(DepositViewModel viewModel)
    {
        viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        viewModel.Account!.AddDeposit(viewModel.Amount, viewModel.Comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Withdraw(int id)
    {
        return View(new WithdrawViewModel()
        {
            Account = await _context.Accounts.FirstAsync(a => a.AccountNumber == id),
            AccountNumber = id
        });
    }

    [HttpPost]
    public async Task<IActionResult> Withdraw(WithdrawViewModel viewModel)
    {
        viewModel.Account = await _context.Accounts.FindAsync(viewModel.AccountNumber);

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (!viewModel.Account!.ValidWithdrawal(viewModel.Amount))
        {
            ModelState.AddModelError(nameof(viewModel.Amount), "You cannot withdraw more than your available balance");
            return View(viewModel);
        }

        viewModel.Account.AddWithdrawal(viewModel.Amount, viewModel.Comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}