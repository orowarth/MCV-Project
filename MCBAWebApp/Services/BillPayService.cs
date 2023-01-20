using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;
using Microsoft.EntityFrameworkCore;
namespace MCBAWebApp.Services;

public class BillPayService : BackgroundService
{
    private readonly IServiceProvider _services;
    public BillPayService(IServiceProvider services) 
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PayBills(stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task PayBills(CancellationToken cancellationToken)
    {
        // Pay some bills...
        using var scope = _services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BankDbContext>();

        var accountsWithBillsDue = await context.Accounts
        .Where(a => a.Bills.Any())
        .Include(a => a.Bills.Where(b => b.ScheduleTimeUtc <= DateTime.UtcNow)).ToListAsync();

        var finishedBills = new List<BillPay>();
        
        foreach (var account in accountsWithBillsDue)
        {
            foreach (var bill in account.Bills) 
            {
                // Bill payment logic + new transaction + updated balance + update bill if monthly
            }
        }
    }
}