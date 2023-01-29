using MCBADataLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace MCBAWebApp.Tests;

public class BillPayControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public BillPayControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase("BillPayDb").Options);
        SeedData.Initialize(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}