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

    [Display(Name = "Account Type")]
    public AccountType AccountType { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    public int FreeTransactions { get; set; } = 2;

    [NotMapped]
    public decimal MinimumOpeningBalance { get; set; }

    [NotMapped]
    public decimal MinimumBalance { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public Customer Customer { get; set; } = null!;

    [InverseProperty("Account")]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

  public void AddDepost(decimal amount, string? comment)
  {
    Transactions.Add(new Transaction
        {
            Amount = amount,
            TransactionType = TransactionType.Deposit,
            Comment = (string.IsNullOrWhiteSpace(comment)) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
        Balance += amount;
  }

  public void AddWithdrawal(decimal amount, string? comment)
    {
        Transactions.Add(new Transaction
        {
            Amount = amount,
            TransactionType = TransactionType.Withdrawal,
            Comment = (string.IsNullOrWhiteSpace(comment)) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
        Balance -= amount;

        if (FreeTransactions == 0)
        {
            AddWithdrawlFee(amount, comment);
        }
        else
        {
            FreeTransactions--;
        }
    }

    public void AddWithdrawlFee(decimal amount, string? comment)
    {
        const decimal withdrawalFee = 0.05m;
        Transactions.Add(new Transaction
        {
            Amount = withdrawalFee,
            TransactionType = TransactionType.ServiceCharge,
            Comment = (string.IsNullOrWhiteSpace(comment)) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
        Balance -= withdrawalFee;
    }

    public bool ValidWithdrawal(decimal amount)
    {
        const decimal withdrawalFee = 0.05m;
        var availableBalance = Balance - MinimumBalance;

        if (availableBalance < amount)
        {
            return false;
        }

        if ((FreeTransactions == 0) && (availableBalance < amount + withdrawalFee))
        {
            return false;
        }

        return true;
    }
}
