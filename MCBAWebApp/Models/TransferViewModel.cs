using MCBADataLibrary.Models;
using MCBAWebApp.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Models;

public class TransferViewModel
{
    public Account? Account { get; set; }
    public int AccountNumber { get; set; }

    [Required]
    [PositiveTwoDecimals(ErrorMessage = "Amount must be positive and have no more than two decimal places")]
    public decimal Amount { get; set; }

    [Required]
    [DisplayName("Destination Account")]
    public int DestinationAccount {  get; set; }

    [MaxLength(30)]
    public string? Comment { get; set; }
}
