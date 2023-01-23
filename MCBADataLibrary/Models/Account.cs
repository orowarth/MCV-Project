using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MCBADataLibrary.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MCBADataLibrary.Models;

public class Account
{

    const decimal WithdrawalFee = 0.05m;
    const decimal TransferFee = 0.1m;

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

    [InverseProperty("Account")]
    public List<BillPay> Bills { get; set; } = new List<BillPay>();

    public void ProcessBill(BillPay bill)
    {
        if ((Balance - MinimumBalance) < bill.Amount)
        {
            bill.BillStatus = BillStatus.Late;
            return;
        }

        Balance -= bill.Amount;
        Transactions.Add(new Transaction
        {
            Amount = bill.Amount,
            TransactionType = TransactionType.BillPay,
            Comment = null,
            TransactionTimeUtc = DateTime.UtcNow,
            DestinationAccountNumber = null,
        });

        if (bill.Period == BillPeriod.Monthly)
        {
            bill.ScheduleTimeUtc = bill.ScheduleTimeUtc.AddMonths(1);
        }
        else
        {
            bill.BillStatus = BillStatus.Complete;
        }
    }

    public void AddDeposit(decimal amount, string? comment)
    {
        Transactions.Add(new Transaction
        {
            Amount = amount,
            TransactionType = TransactionType.Deposit,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
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
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
        Balance -= amount;

        if (FreeTransactions == 0)
            AddFee(WithdrawalFee, comment);
        else
            FreeTransactions--;
    }

    public void SendTransfer(decimal amount, string? comment, int destination)
    {
        Transactions.Add(new Transaction
        {
            Amount = amount,
            TransactionType = TransactionType.Transfer,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
            DestinationAccountNumber = destination
        });
        Balance -= amount;

        if (FreeTransactions == 0)
            AddFee(TransferFee, comment);
        else
            FreeTransactions--;
    }

    public void ReceiveTransfer(decimal amount, string? comment)
    {
        Transactions.Add(new Transaction
        {
            Amount = amount,
            TransactionType = TransactionType.Transfer,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
    }

    public void AddFee(decimal fee, string? comment)
    {
        Balance += fee;
        Transactions.Add(new Transaction
        {
            Amount = fee,
            TransactionType = TransactionType.ServiceCharge,
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
            TransactionTimeUtc = DateTime.UtcNow,
        });
        Balance -= fee;
    }

    public bool ValidWithdrawal(decimal amount)
    {
        if (!ValidAmount(amount)) 
            return false;

        var availableBalance = Balance - MinimumBalance;

        if (availableBalance < amount)
            return false;

        if ((FreeTransactions == 0) && (availableBalance < amount + WithdrawalFee))
            return false;

        return true;
    }
    public bool ValidTransfer(decimal amount)
    {
        if (!ValidAmount(amount)) 
            return false;

        var availableBalance = Balance - MinimumBalance;

        if (availableBalance < amount)
            return false;

        if ((FreeTransactions == 0) && (availableBalance < amount + TransferFee))
            return false;

        return true;
    }

    public bool ValidAmount(decimal amount)
    {
        return decimal.Round(amount, 2) == amount && amount > 0;
    }
}
