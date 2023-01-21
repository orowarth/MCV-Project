using MCBAAdminAPI.Data;
using MCBADataLibrary.Admin.Communication;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminAPI.Controllers;

/// <summary>
/// <c>CustomerController</c> handles admin requests related to customers,
/// in particular, it handles the blocking and unblocking of customers.
/// It aditionally allows administrators to view a list of all <c>Customer</c> objects.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Retrieves all <c>Customer</c> objects from a <c>CustomerRepository</c>
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet("GetAllCustomers")]
    public async Task<IActionResult> GetAllCustomers()
    {
        var allCustomers = await _customerRepository.GetAll();
        return Ok(allCustomers);
    }

    /// <summary>
    /// Retrieves a <c>Customer</c> from a <c>CustomerRepository</c> based on a <c>Customer</c> identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
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


    /// <summary>
    /// Updates a <c>Customer</c> object based on an <c>UpdatedCustomer</c>
    /// </summary>
    /// <param name="customer"></param>
    /// <returns>IActionResult</returns>
    [HttpPut("UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer(UpdatedCustomer customer)
    {
        var retrievedCustomer = await _customerRepository.GetById(customer.CustomerID);

        if (retrievedCustomer is null)
        {
            return NotFound();
        }
        await _customerRepository.UpdateCustomer(customer);
        return Ok();
    }

    /// <summary>
    /// Blocks a <c>Customer</c> based on a <c>Customer</c> identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
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

    /// <summary>
    /// Unblocks a <c>Customer</c> based on a <c>Customer</c> identifier
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
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