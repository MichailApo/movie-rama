using MovieRamaWeb.Domain;
using MovieRamaWeb.Requests;

namespace MovieRamaWeb.Services
{
    public interface IMovieRepository
    {
        Task<int> AddMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetMoviesAsync(MovieListQueryParameters filterParameters);
        Task<IEnumerable<Movie>> GetMoviesByCreatorIdAsync(int creatorId, MovieListQueryParameters filterParameters);
    }
}
