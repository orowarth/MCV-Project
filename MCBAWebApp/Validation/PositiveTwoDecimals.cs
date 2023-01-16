using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Validation;

public class PositiveTwoDecimals : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var number = (decimal)value!;

        if (decimal.Round(number, 2) == number && number > 0)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult(ErrorMessageString);
        };
    }
}
