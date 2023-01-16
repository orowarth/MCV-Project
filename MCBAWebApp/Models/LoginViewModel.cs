using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Models;

public class LoginViewModel
{
    [Required]
    public int LoginID { get; set; }

    [Required]
    public string Password { get; set; }
}