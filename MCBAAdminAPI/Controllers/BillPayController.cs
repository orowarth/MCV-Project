using Microsoft.AspNetCore.Mvc;
using MCBAAdminAPI.Data;

namespace MCBAAdminAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillPayController : ControllerBase
{
    private readonly IBillPayRepository _billPayRepository;
    public BillPayController(IBillPayRepository billPayRepository)
    {
        _billPayRepository = billPayRepository;
    }
    // Bpay repository pattern
    [HttpGet("GetCustomersBills/{customerId}")]
    public async Task<IActionResult> GetCustomersBills(int customerId)
    {
        var customerBills = await _billPayRepository.GetAllByCustomerId(customerId);
        return Ok(customerBills);
    }

    [HttpPut("UnblockBill/{id}")]
    public async Task<IActionResult> UnblockBill(int id)
    {
        await _billPayRepository.UnblockBill(id);
        return Ok();
    }

    [HttpPut("BlockBill/{id}")]
    public async Task<IActionResult> BlockBill(int id)
    {
        await _billPayRepository.BlockBill(id);
        return Ok();
    }
}