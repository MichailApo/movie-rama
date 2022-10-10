using MovieRamaWeb.Domain;
using MovieRamaWeb.Application.Requests;

namespace MovieRamaWeb.Application.Services
{
    public interface IMovieRepository
    {
        Task<int> AddMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetMoviesAsync(MovieListQueryParameters filterParameters);
        Task<IEnumerable<Movie>> GetMoviesByCreatorIdAsync(int creatorId, MovieListQueryParameters filterParameters);
        Task<Movie?> GetMovieByIdAsync(int movieId);
    }
}
