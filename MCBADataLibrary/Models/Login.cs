using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MCBADataLibrary.Models;

public class Login
{
    [Column(TypeName = "char")]
    [StringLength(8, MinimumLength = 8)]
    public required string LoginID { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }
    public Customer Customer { get; set; } = null!;

    [Column(TypeName = "char")]
    [Required, StringLength(94, MinimumLength = 94)]
    public required string PasswordHash { get; set; }
}
