using MCBADataLibrary.Models;

namespace MCBAAdminAPI.Data;

public interface ICustomerRepository
{
    public Task<IEnumerable<Customer>> GetAll();
    public Task<Customer?> GetById(int id);
    public Task UpdateCustomer(Customer customer);
    public Task BlockCustomer(int id);
    public Task UnblockCustomer(int id);

}
