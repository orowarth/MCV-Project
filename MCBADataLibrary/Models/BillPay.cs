using MCBADataLibrary.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MCBADataLibrary.Models;

public class BillPay
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("Bill ID")]
    public int BillPayID { get; set; }

    public Account Account { get; set; } = null!;

    [ForeignKey(nameof(Account))]
    [DisplayName("Account Number")]
    public int AccountNumber { get; set; }

    [ForeignKey(nameof(Payee))]
    public int PayeeID { get; set; }

    public Payee Payee { get; set; } = null!;

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [DisplayName("Scheduled Time")]
    public DateTime ScheduleTimeUtc { get; set; }

    public BillStatus BillStatus { get; set; } = BillStatus.OnTime;

    public BillPeriod Period { get; set; }
}
