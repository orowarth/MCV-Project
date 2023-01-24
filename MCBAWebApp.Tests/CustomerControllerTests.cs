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

    [Fact]
    public async Task DepositPost_WhenValid_ReturnsViewResult()
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Deposit(new DepositViewModel
        {
            Amount = 10,
            AccountNumber = 4100
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.Equal("ConfirmDeposit", viewModel.ViewName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    public async Task DepositPost_WhenInvalid_ReturnsInvalidModelState(decimal amount)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Deposit(new DepositViewModel
        {
            Amount = amount,
            AccountNumber = 4100
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.NotEqual("ConfirmDeposit", viewModel.ViewName);
    }

    [Fact]
    public async Task Withdraw_ReturnsViewResult()
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Withdraw(4100);

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<WithdrawViewModel>(viewModel.Model);
    }

    [Fact]
    public async Task Withdraw_WhenValid_ReturnsViewResult()
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Withdraw(new WithdrawViewModel
        {
            Amount = 10,
            AccountNumber = 4100
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.Equal("ConfirmWithdrawal", viewModel.ViewName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    [InlineData(10000)]
    public async Task Withdraw_WhenInvalid_ReturnsViewResult(decimal amount)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Withdraw(new WithdrawViewModel
        {
            Amount = amount,
            AccountNumber = 4100
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.NotEqual("ConfirmWithdrawal", viewModel.ViewName);
    }
}