using MCBADataLibrary.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBADataLibrary.Models;

public class BillPay
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BillPayID { get; set; }

    public Account Account { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountNumber { get; set; }

    [ForeignKey(nameof(Payee))]
    public int PayeeID { get; set; }

    public Payee Payee { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public BillPeriod Period { get; set; }
}
