using MovieRamaWeb.Domain;
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
                CreatedAt = DateTime.UtcNow,
                Description = movie.Description,
                CreatorId = movie.Creator.Id,
                Title = movie.Title
            };

            var addedMovie = await _dbContext.Movies.AddAsync(movieEntity);
            await _dbContext.SaveChangesAsync();
            return addedMovie.Entity.Id;

        }
    }
}
