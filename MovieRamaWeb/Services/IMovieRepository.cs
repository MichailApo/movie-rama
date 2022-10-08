using MovieRamaWeb.Domain;

namespace MovieRamaWeb.Services
{
    public interface IMovieRepository
    {
        Task<int> AddMovieAsync(Movie movie);
    }
}
