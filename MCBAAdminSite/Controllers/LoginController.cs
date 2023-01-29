using MCBAAdminSite.Models;
using MCBADataLibrary.Admin.Communication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MCBAAdminSite.Controllers;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public LoginController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginInput loginInput)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        var response = await httpClient.PostAsJsonAsync("Admin/Login", loginInput);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return View();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            ModelState.AddModelError("LoginFailure", "Incorrect admin username and/or password.");
            return View();
        }

        HttpContext.Session.SetInt32("Admin", 1);
        return RedirectToAction("Index", "Customer");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}