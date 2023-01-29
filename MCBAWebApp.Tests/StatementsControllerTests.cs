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

public class StatementsControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public StatementsControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase("StatementsDb").Options);
        SeedData.Initialize(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        // Arrange
        var customerController = new StatementsController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        customerController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act
        var result = await customerController.Index();

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        var viewData = Assert.IsAssignableFrom<Customer>(viewModel.Model);
        Assert.Equal(2, viewData.Accounts.Count);
    }

    [Fact]
    public async Task Statements_ReturnsViewResult()
    {
        // Arrange
        var customerController = new StatementsController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        customerController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act
        var result = await customerController.Statement(4100, 1);

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        var viewData = Assert.IsAssignableFrom<StatementsViewModel>(viewModel.Model);
        Assert.Equal(1, viewData.Transactions.Count);
    }
}