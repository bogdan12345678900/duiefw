using System.ComponentModel.DataAnnotations;
using TopMovies.Models;
using TopMovies.Services;

namespace TopMovies.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueMovieTitleAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
    {
        if (value is not string title || string.IsNullOrWhiteSpace(title)) return ValidationResult.Success;
        var svc = (IMovieService)ctx.GetService(typeof(IMovieService))!;
        var id = (ctx.ObjectInstance as Movie)?.Id ?? 0;
        return svc.TitleExists(title.Trim(), id)
            ? new ValidationResult($"Фильм «{title}» уже есть в списке", new[] { ctx.MemberName! })
            : ValidationResult.Success;
    }
}
