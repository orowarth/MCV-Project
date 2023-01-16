using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MCBAWebApp.Models;

public class LoginViewModel
{
    [Required]
    [DisplayName("Login ID")]
    public string LoginID { get; set; }

    [Required]
    public string Password { get; set; }
}