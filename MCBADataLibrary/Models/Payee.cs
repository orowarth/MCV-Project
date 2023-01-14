using MCBADataLibrary.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBADataLibrary.Models;

public class Payee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PayeeID { get; set; }

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [Required, MaxLength(50)]
    public required string Address { get; set; }

    [Required, MaxLength(40)]
    public required string City { get; set; }

    public State State { get; set; }

    [Required, StringLength(4, MinimumLength = 4)]
    public required string PostCode { get; set; }

    [Required, StringLength(14, MinimumLength = 14)]
    [RegularExpression(@"\(0\d\)(\s\d{4}){2}")]
    public required string Phone { get; set; }
}
