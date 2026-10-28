using TopMovies.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddControllersWithViews(o =>
{
    var p = o.ModelBindingMessageProvider;
    p.SetValueMustBeANumberAccessor(f => $"Поле «{f}» должно быть числом");
    p.SetValueIsInvalidAccessor(v => $"Значение «{v}» некорректно");
    p.SetMissingBindRequiredValueAccessor(f => $"Поле «{f}» обязательно");
});
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Movies}/{action=Index}/{id?}");
app.Run();
