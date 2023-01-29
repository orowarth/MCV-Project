using MCBADataLibrary.Admin.Communication;
using MCBADataLibrary.Models;

namespace MCBAAdminAPI.Data;

public interface ICustomerRepository
{
    public Task<IEnumerable<Customer>> GetAll();
    public Task<Customer?> GetById(int id);
    public Task UpdateCustomer(UpdatedCustomer customer);
    public Task BlockCustomer(int id);
    public Task UnblockCustomer(int id);
}
