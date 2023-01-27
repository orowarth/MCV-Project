using MCBADataLibrary.Enums;
using System.ComponentModel.DataAnnotations;

namespace MCBADataLibrary.Admin.Communication;

public class UpdatedCustomer
{
    // Keeps the ID length to 4 digits
    [Range(1000, 9999)]
    public int CustomerID { get; set; }

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [StringLength(maximumLength: 11, MinimumLength = 11)]
    [RegularExpression(@"(\d{3}\s){2}\d{3}", ErrorMessage = "Must be of the format: XXX XXX XXX")]
    public string? TFN { get; set; }

    [MaxLength(50)]
    public string? Address { get; set; }
    [MaxLength(60)]
    public string? City { get; set; }
    public State? State { get; set; }

    [StringLength(maximumLength: 4, MinimumLength = 4)]
    public string? PostCode { get; set; }

    [StringLength(maximumLength: 12, MinimumLength = 12)]
    [RegularExpression(@"04\d{2}(\s\d{3}){2}", ErrorMessage = "Must be of the format: 04XX XXX XXX")]
    public string? Mobile { get; set; }
}
