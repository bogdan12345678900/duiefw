using System.ComponentModel.DataAnnotations;
using TopMovies.Validation;

namespace TopMovies.Models;

public class Movie
{
    public int Id { get; set; }

    [Display(Name = "Позиция в топ-10")]
    [Required(ErrorMessage = "Укажите позицию в рейтинге")]
    [Range(1, 10, ErrorMessage = "Позиция должна быть от 1 до 10")]
    public int? Rank { get; set; }

    [Display(Name = "Название")]
    [Required(ErrorMessage = "Введите название фильма")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название: от {2} до {1} символов")]
    [UniqueMovieTitle]
    public string Title { get; set; } = "";

    [Display(Name = "Режиссёр")]
    [Required(ErrorMessage = "Укажите режиссёра")]
    [RegularExpression(@"^[\p{L}\s\.\-']+$", ErrorMessage = "Только буквы, пробелы, точки и дефисы")]
    [StringLength(80)]
    public string Director { get; set; } = "";

    [Display(Name = "Год выпуска")]
    [Required(ErrorMessage = "Укажите год выпуска")]
    [ReleaseYear(1888)]
    public int? Year { get; set; }

    [Display(Name = "Оценка")]
    [Required(ErrorMessage = "Поставьте оценку")]
    [Range(1.0, 10.0, ErrorMessage = "Оценка должна быть от {1} до {2}")]
    public double? Rating { get; set; }

    [Display(Name = "Описание")]
    [StringLength(1000, ErrorMessage = "Не более {1} символов")]
    [NoForbiddenWords("спойлер", "spoiler", "пиратск")]
    public string? Description { get; set; }

    [Display(Name = "Ссылка на постер")]
    [Url(ErrorMessage = "Введите корректный URL")]
    public string? PosterUrl { get; set; }
}
