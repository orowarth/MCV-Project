using MCBADataLibrary.Data;
using MCBAWebApp.Controllers;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MCBAWebApp.Tests;

public class LoginControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public LoginControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase(nameof(BankDbContext)).Options);
        SeedData.Initialize(_context);
    }

    public void Dispose() => _context.Dispose();
    

    [Fact]
    public void Login_ReturnsAViewResult_WithLoginViewModelAsync()
    {
        var loginController = new LoginController(_context);
        var result = loginController.Login();

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.NotNull(viewResult);
    }
}