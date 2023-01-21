using MCBADataLibrary.Models;

namespace MCBAAdminAPI.Data;

public interface ICustomerRepository
{
    public Task<IEnumerable<Customer>> GetAll();
    public Task<Customer?> GetById(int id);
}
