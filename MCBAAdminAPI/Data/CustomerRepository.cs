using MCBADataLibrary.Admin.Communication;
using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
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

    public async Task UpdateCustomer(UpdatedCustomer customer)
    {
        var retrievedCustomer = await _context.Customers.FindAsync(customer.CustomerID)!;
        retrievedCustomer!.Name = customer.Name;
        retrievedCustomer.TFN = customer.TFN;
        retrievedCustomer.Address = customer.Address;
        retrievedCustomer.City = customer.City;
        retrievedCustomer.State = customer.State;
        retrievedCustomer.PostCode = customer.PostCode;
        retrievedCustomer.Mobile = customer.Mobile;
        await _context.SaveChangesAsync();
    }

    public async Task BlockCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id)!;
        customer!.CustomerStatus = CustomerStatus.Blocked;
        await _context.SaveChangesAsync();
    }

    public async Task UnblockCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id)!;
        customer!.CustomerStatus = CustomerStatus.Unblocked;
        await _context.SaveChangesAsync();
    }
}
