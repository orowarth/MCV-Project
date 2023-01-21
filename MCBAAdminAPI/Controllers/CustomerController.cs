using MCBAAdminAPI.Data;
using MCBAAdminAPI.Communication;
using MCBADataLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("GetAllCustomers")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var allCustomers = await _customerRepository.GetAll();
        return Ok(allCustomers);
    }

    [HttpGet("GetCustomerById/{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetById(id);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPut("UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer(CustomerDto customer)
    {
        var retrievedCustomer = await _customerRepository.GetById(customer.CustomerID);

        if (retrievedCustomer is null)
        {
            return NotFound();
        }
        await _customerRepository.UpdateCustomer(customer);
        return Ok();
    }

    [HttpPut("BlockCustomer/{id}")]
    public async Task<IActionResult> BlockCustomer(int id)
    {
        var retrievedCustomer = await _customerRepository.GetById(id);

        if (retrievedCustomer is null)
        {
            return NotFound();
        }
        
        await _customerRepository.BlockCustomer(id);

        return Ok();
    }
    [HttpPut("UnblockCustomer/{id}")]
    public async Task<IActionResult> UnblockCustomer(int id)
    {
        var retrievedCustomer = await _customerRepository.GetById(id);

        if (retrievedCustomer is null)
        {
            return NotFound();
        }
        
        await _customerRepository.UnblockCustomer(id);

        return Ok();
    }
}