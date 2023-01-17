using MCBADataLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Models;

public class TransferViewModel
{
    public Account? Account { get; set; }
    public int AccountNumber { get; set; }

    [Required]
    public int DestinationAccount {  get; set; }

    [MaxLength(30)]
    public string? Comment { get; set; }
}
