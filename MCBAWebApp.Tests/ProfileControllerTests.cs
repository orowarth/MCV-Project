using MCBADataLibrary.Data;
using MCBADataLibrary.Enums;
using MCBADataLibrary.Models;
using MCBAWebApp.Controllers;
using MCBAWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace MCBAWebApp.Tests;

public class ProfileControllerTests : IDisposable
{
    private readonly BankDbContext _context;

    public ProfileControllerTests()
    {
        _context = new BankDbContext(new DbContextOptionsBuilder<BankDbContext>().
            UseInMemoryDatabase("ProfileDb").Options);
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
        var profileController = new ProfileController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        profileController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act 
        var result = await profileController.Index();

        // Assert
        var viewModel = Assert.IsType<ViewResult>(result);
        var viewData = Assert.IsType<Customer>(viewModel.Model);
        Assert.Equal("Test", viewData.Name);
    }

    [Fact]
    public async Task EditProfile_ReturnsAViewResult()
    {
        // Arrange 
        var profileController = new ProfileController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        profileController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act 
        var result = await profileController.EditProfile();

        // Assert 
        var viewResult = Assert.IsType<ViewResult>(result);
        var viewModel = Assert.IsType<ProfileViewModel>(viewResult.ViewData.Model);
        Assert.Equal("Test", viewModel.Name);
    }

    [Fact]
    public async Task EditProfile_WhenValid_ReturnsRedirectToActionResult()
    {
        // Arrange 
        var profileController = new ProfileController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        profileController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        // Act 
        var result = await profileController.EditProfile(new ProfileViewModel()
        {
            Name = "Test2",
            Address = "321 Test Road",
            State = State.QLD,
            City = "Cairns",
            Mobile = "0400 000 000",
            PostCode = "0000",
            TFN = "000 000 000"
        });

        // Assert 
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task EditProfile_WhenInvalid_ReturnInvalidModelState()
    {
        // Arrange 
        var profileController = new ProfileController(_context);
        var sessionMock = new Mock<ISession>();
        var key = nameof(Customer.CustomerID);
        var value = BitConverter.GetBytes(2100);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(value);

        sessionMock.Setup(s => s.TryGetValue(key, out value)).Returns(true);

        profileController.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            Session = sessionMock.Object,
        };

        var profileViewModel = new ProfileViewModel()
        {
            Name = "",

            // 51 characters
            Address = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",

            // 61 characters
            City = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            PostCode = "123",

            // Both mobile and TFN will count two errors as they fail two validation annotations
            Mobile = "123",
            TFN = "123",
        };

        // Act
        SimulateProfileValidation(profileController, profileViewModel);
        var result = await profileController.EditProfile(profileViewModel);

        // Assert 
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(8, viewResult.ViewData.ModelState.ErrorCount);
    }

    // Thanks to a handy article from https://stackoverflow.com/a/37558050
    private void SimulateProfileValidation(ProfileController controller, ProfileViewModel profileViewModel)
    {
        var validationContext = new ValidationContext(profileViewModel, null, null);
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(profileViewModel, validationContext, validationResults, true);
        foreach (var validationResult in validationResults)
        {
            controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
        }
    }
}