using System.ComponentModel.DataAnnotations;

namespace MCBAWebApp.Validation;

public class FutureTime : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dateTime = (DateTime)value!;

        if (dateTime > DateTime.Now)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult(ErrorMessageString);
        };
    }
}
