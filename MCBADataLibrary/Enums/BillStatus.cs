using System.ComponentModel.DataAnnotations;

namespace MCBADataLibrary.Enums;

public enum BillStatus
{
    [Display(Name = "On time")]
    OnTime = 1,
    [Display(Name = "Overdue")]
    Late = 2,
    Complete = 3,
}
