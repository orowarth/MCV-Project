using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCBADataLibrary.Enums;

namespace MCBADataLibrary.Models;

public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionID { get; set; }
    public TransactionType TransactionType { get; set; }

    [ForeignKey(nameof(Account))]
    public int AccountNumber { get; set; }
    public Account Account { get; set; }

    [ForeignKey("DestinationAccount")]
    public int? DestinationAccountNumber { get; set; }
    public Account DestinationAccount { get; set; }

    [Column(TypeName = "money")]
    public decimal Amount { get; set; }

    [StringLength(30)]
    public string? Comment { get; set; }

    public DateTime TransactionTimeUtc { get; set; }
}
