using TopMovies.Models;

namespace TopMovies.Services;

public interface IMovieService
{
    IEnumerable<Movie> GetAll();
    Movie? Get(int id);
    void Add(Movie m);
    void Update(Movie m);
    void Delete(int id);
    bool TitleExists(string title, int excludeId);
    bool RankTaken(int rank, int excludeId);
    int Count { get; }
}

public class MovieService : IMovieService
{
    private readonly List<Movie> _movies = new()
    {
        new() { Id = 1, Rank = 1, Title = "Побег из Шоушенка", Director = "Фрэнк Дарабонт", Year = 1994, Rating = 9.3, Description = "История дружбы и надежды в тюрьме." },
        new() { Id = 2, Rank = 2, Title = "Крёстный отец", Director = "Фрэнсис Форд Коппола", Year = 1972, Rating = 9.2, Description = "Сага о мафиозной семье Корлеоне." },
        new() { Id = 3, Rank = 3, Title = "Тёмный рыцарь", Director = "Кристофер Нолан", Year = 2008, Rating = 9.0, Description = "Бэтмен против Джокера." },
    };
    private int _nextId = 4;
    private readonly object _lock = new();

    public int Count { get { lock (_lock) return _movies.Count; } }
    public IEnumerable<Movie> GetAll() { lock (_lock) return _movies.OrderBy(m => m.Rank).ToList(); }
    public Movie? Get(int id) { lock (_lock) return _movies.FirstOrDefault(m => m.Id == id); }
    public void Add(Movie m) { lock (_lock) { m.Id = _nextId++; _movies.Add(m); } }
    public void Update(Movie m)
    {
        lock (_lock)
        {
            var i = _movies.FindIndex(x => x.Id == m.Id);
            if (i >= 0) _movies[i] = m;
        }
    }
    public void Delete(int id) { lock (_lock) _movies.RemoveAll(m => m.Id == id); }
    public bool TitleExists(string t, int ex) { lock (_lock) return _movies.Any(m => m.Id != ex && m.Title.Equals(t, StringComparison.OrdinalIgnoreCase)); }
    public bool RankTaken(int r, int ex) { lock (_lock) return _movies.Any(m => m.Id != ex && m.Rank == r); }
}
