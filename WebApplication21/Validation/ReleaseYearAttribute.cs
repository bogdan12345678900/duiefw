using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TopMovies.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class ReleaseYearAttribute : ValidationAttribute, IClientModelValidator
{
    public int MinYear { get; }
    public int MaxYear => DateTime.Now.Year + 1;

    public ReleaseYearAttribute(int minYear = 1888)
    {
        MinYear = minYear;
        ErrorMessage = "Поле «{0}»: год от {1} до {2}";
    }

    public override string FormatErrorMessage(string name) =>
        string.Format(ErrorMessageString, name, MinYear, MaxYear);

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is null) return ValidationResult.Success;
        return value is int y && y >= MinYear && y <= MaxYear
            ? ValidationResult.Success
            : new ValidationResult(FormatErrorMessage(ctx.DisplayName));
    }

    public void AddValidation(ClientModelValidationContext c)
    {
        Add(c, "data-val", "true");
        Add(c, "data-val-releaseyear", FormatErrorMessage(c.ModelMetadata.GetDisplayName()));
        Add(c, "data-val-releaseyear-min", MinYear.ToString());
        Add(c, "data-val-releaseyear-max", MaxYear.ToString());
    }

    private static void Add(ClientModelValidationContext c, string k, string v)
    {
        if (!c.Attributes.ContainsKey(k)) c.Attributes.Add(k, v);
    }
}
