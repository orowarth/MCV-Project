using MCBADataLibrary.Models;

namespace MCBAAdminSite.Models;

public class BillViewModel
{
    public List<BillPay> Bills { get; set; }
    public int CustomerID { get; set; }
}
