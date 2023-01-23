using MCBADataLibrary.Data;
using MCBAWebApp.Controllers;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace MCBAWebApp.Tests;

public class LoginControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public LoginControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase("LoginDb").Options);
        SeedData.Initialize(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }


    [Fact]
    public void Login_ReturnsAViewResult()
    {
        // Arrange
        var loginController = new LoginController(_context);
        
        // Act
        var result = loginController.Login();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult);
        Assert.True(viewResult.ViewData.ModelState.IsValid);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("1234", "")]
    [InlineData("", "abc")]
    [InlineData("1234", "abc")]
    public async Task Login_ReturnsAViewResult_ModelStateIsInvalid(string loginId, string password)
    {
        // Arrange
        var loginController = new LoginController(_context);

        // Act
        var result = await loginController.Login(new LoginViewModel
        {
            LoginID = loginId,
            Password = password
        });

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(viewResult.ViewData.ModelState.IsValid);
    }

    [Fact]
    public async Task Login_ReturnsARedirectAction_ValidLogin()
    {
        // Arrange
        var mockSession = new Mock<ISession>();
        var loginController = new LoginController(_context);

        // Act
        loginController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            Session = mockSession.Object
        };

        var result = await loginController.Login(new LoginViewModel
        {
            LoginID = "12345678",
            Password = "abc123"
        });

        // Assert
        var actionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(actionResult);
        Assert.Equal("Customer", actionResult.ControllerName);
    }
}