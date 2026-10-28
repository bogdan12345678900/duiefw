using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TopMovies.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class NoForbiddenWordsAttribute : ValidationAttribute, IClientModelValidator
{
    private readonly string[] _words;

    public NoForbiddenWordsAttribute(params string[] words)
    {
        _words = words;
        ErrorMessage = "Поле «{0}» содержит запрещённое слово";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is not string t || string.IsNullOrWhiteSpace(t)) return ValidationResult.Success;
        return _words.Any(w => t.Contains(w, StringComparison.OrdinalIgnoreCase))
            ? new ValidationResult(FormatErrorMessage(ctx.DisplayName))
            : ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext c)
    {
        void Add(string k, string v) { if (!c.Attributes.ContainsKey(k)) c.Attributes.Add(k, v); }
        Add("data-val", "true");
        Add("data-val-noforbiddenwords", FormatErrorMessage(c.ModelMetadata.GetDisplayName()));
        Add("data-val-noforbiddenwords-words", string.Join(",", _words));
    }
}
