using MCBAAdminSite.Filters;
using MCBAAdminSite.Models;
using MCBADataLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminSite.Controllers;

[AuthorizeAdmin]
public class BillPayController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BillPayController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }


    public async Task<IActionResult> Index(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        var response = await httpClient.GetFromJsonAsync<List<BillPay>>($"BillPay/GetCustomersBills/{id}");
        return View(new BillViewModel
        {
            Bills = response!,
            CustomerID = id,
        });
    }

    public async Task<IActionResult> BlockBill(int customerId, int billId)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        await httpClient.PutAsync($"BillPay/BlockBill/{billId}", null);
        return RedirectToAction("Index", new { id = customerId });
    }

    public async Task<IActionResult> UnblockBill(int customerId, int billId)
    {
        var httpClient = _httpClientFactory.CreateClient("Admin");
        await httpClient.PutAsync($"BillPay/UnblockBill/{billId}", null);
        return RedirectToAction("Index", new { id = customerId });

    }
}