using System.ComponentModel.DataAnnotations;

namespace MCBADataLibrary.Admin.Communication;
public class LoginInput
{
    [Required]
    public required string LoginID { get; set; }

    [Required]
    public required string Password { get; set; }

}