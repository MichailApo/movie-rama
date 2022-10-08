using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Domain;
using MovieRamaWeb.Domain.Enums;
using MovieRamaWeb.Requests;
using MovieRamaWeb.Services;

namespace MovieRamaWeb.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieRamaDbContext _dbContext;

        public MovieRepository(MovieRamaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddMovieAsync(Movie movie)
        {
            var movieEntity = new MovieEntity()
            {
                CreatedAt = movie.PublishedAt,
                Description = movie.Description,
                CreatorId = movie.Creator.Id,
                Title = movie.Title
            };

            var addedMovie = await _dbContext.Movies.AddAsync(movieEntity);
            await _dbContext.SaveChangesAsync();
            return addedMovie.Entity.Id;

        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(MovieListQueryParameters filterParameters)
        {
            return await ApplyMovieOrdering(_dbContext.Movies.AsNoTracking(), filterParameters)
                .Select(m => Movie.Create(m.Title, m.Description, new Domain.User(m.Creator.Id, m.Creator.UserName), m.CreatedAt))
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByCreatorIdAsync(int creatorId, MovieListQueryParameters filterParameters)
        {
            var filteredMovies = _dbContext.Movies.Where(m => m.CreatorId == creatorId);
            return await ApplyMovieOrdering(filteredMovies, filterParameters)
                .Select(m => Movie.Create(m.Title, m.Description, new Domain.User(m.Creator.Id, m.Creator.UserName), m.CreatedAt))
                .ToArrayAsync();
        }

        private IQueryable<MovieEntity> ApplyMovieOrdering(IQueryable<MovieEntity> movies, MovieListQueryParameters filterParameters)
        {
            return (filterParameters.SortType, filterParameters.SortOrder) switch
            {
                (MovieSortType.Date, SortOrder.Asc) => movies.AsNoTracking().OrderBy(m => m.CreatedAt),
                (MovieSortType.Date, SortOrder.Desc) => movies.AsNoTracking().OrderByDescending(m => m.CreatedAt),
                (_, _) => movies.AsNoTracking().OrderByDescending(m => m.CreatedAt)
            };
        }
    }
}
