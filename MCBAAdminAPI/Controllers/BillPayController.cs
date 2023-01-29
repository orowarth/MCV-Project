using MCBAAdminAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminAPI.Controllers;

/// <summary>
/// <c>BillPayController</c> handles requests related to bill payments,
/// in particular, it handles the blocking and unblocking of customer bill payments.
/// It aditionally allows administrators to view all <c>Customer</c> Bills.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BillPayController : ControllerBase
{
    private readonly IBillPayRepository _billPayRepository;
    public BillPayController(IBillPayRepository billPayRepository)
    {
        _billPayRepository = billPayRepository;
    }
    
    /// <summary>
    /// Retrieves all bills for a customer with a given <c>Customer</c> identifier
    /// </summary>
    /// <param name="customerId">Customer identifier</param>
    /// <returns></returns>
    [HttpGet("GetCustomersBills/{customerId}")]
    public async Task<IActionResult> GetCustomersBills(int customerId)
    {
        var customerBills = await _billPayRepository.GetAllByCustomerId(customerId);
        return Ok(customerBills);
    }

    /// <summary>
    /// Unblocks a bill based on a <c>BillPay</c> identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns><c>IActionResult</c></returns>
    [HttpPut("UnblockBill/{id}")]
    public async Task<IActionResult> UnblockBill(int id)
    {
        await _billPayRepository.UnblockBill(id);
        return Ok();
    }

    /// <summary>
    /// Blocks a bill based on a <c>BillPay</c> identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns><c>IActionResult</c></returns>
    [HttpPut("BlockBill/{id}")]
    public async Task<IActionResult> BlockBill(int id)
    {
        await _billPayRepository.BlockBill(id);
        return Ok();
    }
}