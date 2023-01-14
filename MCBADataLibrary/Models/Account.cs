using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCBADataLibrary.Enums;

namespace MCBADataLibrary.Models;

public class Account
{
    public Account(AccountType accountType)
    {
        AccountType = accountType;
        MinimumOpeningBalance = (accountType == AccountType.Savings) ? 50 : 500;
        MinimumBalance = (accountType == AccountType.Savings) ? 0 : 300;
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Account Number")]
    public int AccountNumber { get; set; }

    [Display(Name = "Type")]
    public AccountType AccountType { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    [NotMapped]
    public decimal MinimumOpeningBalance { get; set; }

    [NotMapped]
    public decimal MinimumBalance { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public Customer Customer { get; set; } = null!;

    [InverseProperty("Account")]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}
