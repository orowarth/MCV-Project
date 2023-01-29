using MCBAAdminSite.Filters;
using MCBADataLibrary.Admin.Communication;
using MCBADataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MCBAAdminSite.Controllers;

[AuthorizeAdmin]
public class CustomerController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CustomerController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        var response = await httpClient.GetFromJsonAsync<List<Customer>>("Customer/GetAllCustomers");
        return View(response);
    }

    public async Task<IActionResult> UpdateCustomer(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        var customer = await httpClient.GetFromJsonAsync<UpdatedCustomer>($"Customer/GetCustomerById/{id}");
        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCustomer(UpdatedCustomer customer)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        var response = await httpClient.PutAsJsonAsync("Customer/UpdateCustomer", customer);

        /*
         * Here we are just demonstrating how thee API handles model validation rather 
         * than this MVC application
         */ 
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return View(customer);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UnblockCustomer(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        await httpClient.PutAsync($"Customer/UnblockCustomer/{id}", null);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> BlockCustomer(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        await httpClient.PutAsync($"Customer/BlockCustomer/{id}", null);
        return RedirectToAction(nameof(Index));
    }
}