using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Controllers;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace MCBAWebApp.Tests;

public class CustomerControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public CustomerControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase("CustomerDb").Options);
        SeedData.Initialize(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


    [Fact]
    public async Task Index_ReturnsAViewResult_WithCustomerModel()
    {
        // Arrange
        var customerController = new CustomerController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        int fy = 2100;
        var value = new byte[]
        {
            (byte)(fy >> 24),
            (byte)(0xFF & (fy >> 16)),
            (byte)(0xFF & (fy >> 8)),
            (byte)(0xFF & fy)
        };
        sessionMock.Setup(_ => _.TryGetValue(key, out value)).Returns(true);

        customerController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act
        var result = await customerController.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Customer>(viewResult.Model);
        Assert.Single(model.Accounts);
    }

    [Fact]
    public async Task Deposit_ReturnsViewResult()
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Deposit(4100);

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<DepositViewModel>(viewModel.Model);
    }
}