using System.ComponentModel.DataAnnotations;

namespace MCBAAdminAPI.Communication;
public class LoginDto
{
    [Required]
    public required string LoginID { get; set; }

    [Required]
    public required string Password { get; set; }

}