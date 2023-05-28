using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter;

public class CurrencyCodeAttribute : ValidationAttribute
{
    public string[] _acceptedCodes;
    public CurrencyCodeAttribute(params string[] acceptedCodes)
    {
        _acceptedCodes = acceptedCodes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string code && !(_acceptedCodes.Contains(code)))
        {
            return new ValidationResult("Not allowed currency code");
        }
        return ValidationResult.Success;
    }
}
