using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Domain;
using MovieRamaWeb.Domain.Enums;
using MovieRamaWeb.Application.Requests;
using MovieRamaWeb.Application.Services;
using PreferenceType = Domain.Enums.PreferenceType;

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

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            
            return await MapEntities(_dbContext.Movies).FirstOrDefaultAsync(m => m.Id == movieId);

        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(MovieListQueryParameters filterParameters)
        {
            var projetions = await MapEntities(_dbContext.Movies).ToListAsync();

            return ApplyMovieOrdering(projetions, filterParameters);

        }

        public async Task<IEnumerable<Movie>> GetMoviesByCreatorIdAsync(int creatorId, MovieListQueryParameters filterParameters)
        {
            var projetions = await MapEntities(_dbContext.Movies.Where(m => m.CreatorId == creatorId)).ToListAsync();

            return ApplyMovieOrdering(projetions, filterParameters);
        }


        private IEnumerable<Movie> ApplyMovieOrdering(IEnumerable<Movie> movies, MovieListQueryParameters filterParameters)
        {
            return (filterParameters.SortType, filterParameters.SortOrder) switch
            {
                (MovieSortType.Date, SortOrder.Asc) => movies.OrderBy(m => m.PublishedAt),
                (MovieSortType.Date, SortOrder.Desc) => movies.OrderByDescending(m => m.PublishedAt),
                (MovieSortType.Likes, SortOrder.Asc) => movies.OrderBy(m => m.NumberOfLikes),
                (MovieSortType.Likes, SortOrder.Desc) => movies.OrderByDescending(m => m.NumberOfLikes),
                (MovieSortType.Hates, SortOrder.Asc) => movies.OrderBy(m => m.NumberOfHates),
                (MovieSortType.Hates, SortOrder.Desc) => movies.OrderByDescending(m => m.NumberOfHates),
                (_, _) => movies.OrderByDescending(m => m.PublishedAt)
            };
        }

        private IQueryable<Movie> MapEntities(IQueryable<MovieEntity> movies) => movies.AsNoTracking()
                .Select(m =>
                Movie.Create(
                    m.Id,
                    m.Title,
                    m.Description,
                    new Domain.User(m.Creator.Id, m.Creator.UserName),
                    m.CreatedAt,
                    m.Reactions.Count(r => r.Active && r.Preference == PreferenceType.Like),
                    m.Reactions.Count(r => r.Active && r.Preference == PreferenceType.Hate)
                    )
                );
    }
}
