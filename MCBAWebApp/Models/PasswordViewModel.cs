using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Models;

public class PasswordViewModel
{
    [Required]
    public required string OldPassword { get; set; }

    [Required]
    public required string NewPassword { get; set; }

    [Required]
    public required string NewPasswordRepeat { get; set; }

}
