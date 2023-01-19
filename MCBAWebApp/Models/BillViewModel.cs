using MCBADataLibrary.Enums;
using MCBAWebApp.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace MCBAWebApp.Models;

public class BillViewModel
{
    public IEnumerable<SelectListItem>? Accounts { get; set; }
    public IEnumerable<SelectListItem>? Payees { get; set; }
    public int SelectedAccount { get; set; }
    public int SelectedPayee { get; set; }

    [Required]
    [PositiveTwoDecimals(ErrorMessage = "Amount must be positive and have no more than two decimal places")]
    public decimal Amount { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
    [DataType(DataType.DateTime)]
    public DateTime ScheduleTime { get; set; }

    public BillPeriod Period { get; set; }
}