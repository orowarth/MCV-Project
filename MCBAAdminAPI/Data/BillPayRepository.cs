using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MCBAAdminAPI.Data;

public class BillPayRepository : IBillPayRepository
{
    private readonly BankDbContext _context;

    public BillPayRepository(BankDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BillPay>> GetAllByCustomerId(int id)
    {
        return await _context.BillPayments
            .Where(b => b.Account.CustomerID == id)
            .ToListAsync();
    }

    public async Task BlockBill(int id)
    {
        var billToBlock = await _context.BillPayments.FindAsync(id);
        billToBlock.BillStatus = BillStatus.Blocked;
    }

    public async Task UnblockBill(int id)
    {
        var billToBlock = await _context.BillPayments.FindAsync(id);
        billToBlock.BillStatus = BillStatus.OnTime;
    }
}
