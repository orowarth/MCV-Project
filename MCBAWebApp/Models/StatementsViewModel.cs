using MCBADataLibrary.Models;
using X.PagedList;

namespace MCBAWebApp.Models;

public class StatementsViewModel
{
    public required Account Account { get; set; }
    public required IPagedList<Transaction> Transactions { get; set; }
}
