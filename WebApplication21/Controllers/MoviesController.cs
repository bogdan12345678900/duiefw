using Microsoft.AspNetCore.Mvc;
using TopMovies.Models;
using TopMovies.Services;

namespace TopMovies.Controllers;

public class MoviesController : Controller
{
    private readonly IMovieService _svc;
    public MoviesController(IMovieService svc) => _svc = svc;

    public IActionResult Index() => View(_svc.GetAll());

    public IActionResult Create()
    {
        if (_svc.Count >= 10)
        {
            TempData["Error"] = "В десятке уже 10 фильмов. Удалите один, чтобы добавить новый.";
            return RedirectToAction(nameof(Index));
        }
        return View(new Movie());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(Movie movie)
    {
        if (_svc.Count >= 10)
            ModelState.AddModelError("", "В десятке уже 10 фильмов.");
        CheckRank(movie);
        if (!ModelState.IsValid) return View(movie);
        _svc.Add(movie);
        TempData["Success"] = "Фильм добавлен в десятку лучших!";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var m = _svc.Get(id);
        return m is null ? NotFound() : View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(Movie movie)
    {
        CheckRank(movie);
        if (!ModelState.IsValid) return View(movie);
        _svc.Update(movie);
        TempData["Success"] = "Изменения сохранены.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _svc.Delete(id);
        TempData["Success"] = "Фильм удалён.";
        return RedirectToAction(nameof(Index));
    }

    private void CheckRank(Movie m)
    {
        if (m.Rank is int r && _svc.RankTaken(r, m.Id))
            ModelState.AddModelError(nameof(Movie.Rank), $"Позиция №{r} уже занята другим фильмом");
    }
}
