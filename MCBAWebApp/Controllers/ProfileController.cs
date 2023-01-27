using ImageMagick;
using MCBADataLibrary.Data;
using MCBADataLibrary.Models;
using MCBAWebApp.Filters;
using MCBAWebApp.Models;
using MCBAWebApp.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing.Net;

namespace MCBAWebApp.Controllers;

[AuthorizeCustomer]
public class ProfileController : Controller
{
    private readonly BankDbContext _context;
    private static readonly SimpleHash simpleHash = new();

    public ProfileController(BankDbContext context) => _context = context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID))!.Value;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.FindAsync(CustomerID);
        return View(customer);
    }

    public async Task<IActionResult> EditProfile()
    {
        var customer = await _context.Customers.FindAsync(CustomerID);
        return View(new ProfileViewModel()
        {
            Name = customer!.Name,
            TFN = customer.TFN,
            Address = customer.Address,
            City = customer.City,
            State = customer.State,
            PostCode = customer.PostCode,
            Mobile = customer.Mobile
        });

    }

    public async Task<IActionResult> ChangeImage()
    {
        var customerImage = await _context.CustomerImages.FirstOrDefaultAsync(i => i.CustomerID == CustomerID);
        return View(new ImageViewModel
        {
            CurrentImage = customerImage
        });
    }

    [HttpPost]
    public async Task<IActionResult> ChangeImage(ImageViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        string? image = null;
        if (viewModel.Image is not null)
        {
            var maxSize = new MagickGeometry(400, 400)
            {
                IgnoreAspectRatio = false,
                FillArea = true,
            };

            using var magickImage = new MagickImage(await viewModel.Image.GetBytes());
            magickImage.Format = MagickFormat.Jpg;
            magickImage.Resize(maxSize);
            image = magickImage.ToBase64();
        }

        var customerImage = await _context.CustomerImages
            .FirstOrDefaultAsync(i => i.CustomerID == CustomerID);

        if (customerImage is null)
        {
            _context.CustomerImages.Add(new CustomerImage
            {
                CustomerID = CustomerID,
                Image = image
            });
        }
        else
        {
            customerImage.Image = image;
        }

        if (image is not null)
        {
            HttpContext.Session.SetString(nameof(Customer.Image), image!);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteImage()
    {
        var customerImage = await _context.CustomerImages.FirstOrDefaultAsync(i => i.CustomerID == CustomerID);
        if (customerImage is not null)
        {
            _context.CustomerImages.Remove(customerImage);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove(nameof(Customer.Image));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(ProfileViewModel profile)
    {

        if (!ModelState.IsValid)
        {
            return View(profile);
        }

        var customer = await _context.Customers
            .FirstAsync(c => c.CustomerID == CustomerID);
        customer!.Name = profile.Name;
        customer.Address = profile.Address;
        customer.City = profile.City;
        customer.State = profile.State;
        customer.PostCode = profile.PostCode;
        customer.Mobile = profile.Mobile;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(PasswordViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        bool validSubmission = true;

        var login = await _context.Logins.FirstAsync(l => l.CustomerID == CustomerID);

        if (!simpleHash.Verify(viewModel.OldPassword, login.PasswordHash))
        {
            ModelState.AddModelError(nameof(PasswordViewModel.OldPassword), "Password is incorrect.");
            validSubmission = false;
        }

        if (viewModel.NewPassword != viewModel.NewPasswordRepeat)
        {
            ModelState.AddModelError(nameof(PasswordViewModel.NewPassword), "Passwords do not match.");
            ModelState.AddModelError(nameof(PasswordViewModel.NewPasswordRepeat), "Passwords do not match.");
            validSubmission = false;
        }

        if (!validSubmission)
        {
            return View(viewModel);
        }

        login.PasswordHash = simpleHash.Compute(viewModel.NewPassword);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}