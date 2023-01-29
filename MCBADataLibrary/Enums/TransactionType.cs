using System.ComponentModel.DataAnnotations;

namespace MCBADataLibrary.Enums;

public enum TransactionType
{
    Deposit = 1,
    Withdrawal = 2,
    Transfer = 3,

    [Display(Name = "Service Charge")]
    ServiceCharge = 4,

    [Display(Name = "Bill Payment")]
    BillPay = 5,
}
