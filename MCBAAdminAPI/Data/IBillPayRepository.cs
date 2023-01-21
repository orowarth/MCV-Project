using MCBADataLibrary.Models;

namespace MCBAAdminAPI.Data;

public interface IBillPayRepository
{
    public Task<IEnumerable<BillPay>> GetAllByCustomerId(int id);
    public Task BlockBill(int id);
    public Task UnblockBill(int id);
}
