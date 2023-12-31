﻿using MCBADataLibrary.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCBADataLibrary.Models;

public class Customer
{
    // Keeps the ID length to 4 digits
    [Range(1000, 9999)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [DisplayName("ID")]
    public int CustomerID { get; set; }

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [StringLength(maximumLength: 11, MinimumLength = 11)]
    [RegularExpression(@"(\d{3}\s){2}\d{3}")]
    public string? TFN { get; set; }

    [MaxLength(50)]
    public string? Address { get; set; }
    [MaxLength(60)]
    public string? City { get; set; }
    public State? State { get; set; }

    [StringLength(maximumLength: 4, MinimumLength = 4)]
    public string? PostCode { get; set;}
    
    [StringLength(maximumLength: 12, MinimumLength = 12)]
    [RegularExpression(@"04\d{2}(\s\d{3}){2}")]
    public string? Mobile { get; set; }

    [DisplayName("Status")]
    public CustomerStatus CustomerStatus { get; set; } = CustomerStatus.Unblocked;

    public Login Login { get; set; } = null!;

    public CustomerImage? Image { get; set; }

    public List<Account> Accounts { get; set; } = new List<Account>();
}
