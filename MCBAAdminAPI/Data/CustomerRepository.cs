using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MCBAAdminAPI.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly BankDbContext _context;

    public CustomerRepository(BankDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetById(int id)
    {
        return await _context.Customers.FindAsync(id);
    }
}
