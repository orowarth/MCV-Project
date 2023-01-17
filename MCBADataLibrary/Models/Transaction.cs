using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCBADataLibrary.Enums;
using System.ComponentModel;

namespace MCBADataLibrary.Models;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("Transaction ID")]
    public int TransactionID { get; set; }

    [DisplayName("Type")]
    public TransactionType TransactionType { get; set; } = TransactionType.Deposit;

    [ForeignKey(nameof(Account))]
    [DisplayName("Account Number")]
    public int AccountNumber { get; set; }
    public Account Account { get; set; }

    [ForeignKey("DestinationAccount")]
    [DisplayName("Destination Account")]
    public int? DestinationAccountNumber { get; set; }
    public Account DestinationAccount { get; set; } = null!;

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [StringLength(30)]
    public string? Comment { get; set; }

    [DisplayName("Transaction Time")]
    public DateTime TransactionTimeUtc { get; set; }
}
