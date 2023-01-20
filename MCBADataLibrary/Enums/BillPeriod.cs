using System.ComponentModel.DataAnnotations;
namespace MCBADataLibrary.Enums;

public enum BillPeriod
{
    [Display(Name = "Once Off")]
    OnceOff = 1,
    Monthly = 2
}
