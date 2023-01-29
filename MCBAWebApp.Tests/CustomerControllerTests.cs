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
        Assert.NotEmpty(model.Accounts);
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
    public async Task Deposit_ValidAmount_ReturnsViewResult()
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
        Assert.True(viewModel.ViewData.ModelState.IsValid);
        Assert.Equal("ConfirmDeposit", viewModel.ViewName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    public async Task Deposit_InvalidAmountFormat_ReturnsInvalidModelState(decimal amount)
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
        Assert.False(viewModel.ViewData.ModelState.IsValid);
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

    [Theory]
    [InlineData(10, 4100)]
    [InlineData(100, 4100)]
    [InlineData(700, 4101)]
    public async Task Withdraw_ValidAmount_ReturnsViewResult(decimal amount, int accountNumber)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Withdraw(new WithdrawViewModel
        {
            Amount = amount,
            AccountNumber = accountNumber
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.True(viewModel.ViewData.ModelState.IsValid);
        Assert.Equal("ConfirmWithdrawal", viewModel.ViewName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0.001)]
    public async Task Withdraw_InvalidAmountFormat_ReturnsInvalidModelState(decimal amount)
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
        Assert.False(viewModel.ViewData.ModelState.IsValid);
        Assert.NotEqual("ConfirmWithdrawal", viewModel.ViewName);
    }

    [Theory]
    [InlineData(100.01, 4100)]
    [InlineData(1000.01, 4101)]
    // Checking accounts can go no lower than $300 as per business rules, so ($1000 - $700.01 = $299.99) should be invalid
    [InlineData(700.01, 4101)]
    public async Task Withdraw_AmountMoreThanBalance_ReturnsInvalidModelState(decimal amount, int accountNumber)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Withdraw(new WithdrawViewModel
        {
            Amount = amount,
            AccountNumber = accountNumber
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.False(viewModel.ViewData.ModelState.IsValid);
        Assert.NotEqual("ConfirmWithdrawal", viewModel.ViewName);
    }

    [Fact] 
    public async Task Transfer_ReturnsViewModel()
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Transfer(4100);

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<TransferViewModel>(viewModel.Model);
    }

    [Theory]
    [InlineData(4100, 0)]
    [InlineData(4100, 1)]
    [InlineData(4100, 4100)]
    public async Task Transfer_WhenInvalidDestination_ReturnsInvalidModelState(int senderAccount, int destinationAccountNumber)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Transfer(new TransferViewModel
        {
            AccountNumber = senderAccount,
            DestinationAccount = destinationAccountNumber,
            Amount = 1
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.False(viewModel.ViewData.ModelState.IsValid);
        Assert.NotEqual("ConfirmTransfer", viewModel.ViewName);
    }

    [Theory]
    [InlineData(4100, 4101)]
    [InlineData(4101, 4100)]
    public async Task Transfer_WhenValidDestination_ReturnsViewResult(int senderAccount, int destinationAccountNumber)
    {
        // Arrange
        var customerController = new CustomerController(_context);

        // Act
        var result = await customerController.Transfer(new TransferViewModel
        {
            AccountNumber = senderAccount,
            DestinationAccount = destinationAccountNumber,
            Amount = 1
        });

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        Assert.True(viewModel.ViewData.ModelState.IsValid);
        Assert.Equal("ConfirmTransfer", viewModel.ViewName);
    }
}