using MCBAAdminAPI.Data;
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
}