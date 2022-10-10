using FluentAssertions;
using Infrastructure.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Data;
using MovieRamaWeb.Data.Repositories;
using MovieRamaWeb.Domain;
using Domain.Entities;
using User = MovieRamaWeb.Data.User;

namespace Infrastructure.Sql.Integration
{
    public class MovieRepository_Tests
    {
        private MovieRamaDbContext _dbContext;
        private MovieRepository _MovieReposut;
        private ReactionRepository _ReactionRepoSut;
        public MovieRepository_Tests()
        {
            var options = new DbContextOptionsBuilder<MovieRamaDbContext>()
            .UseInMemoryDatabase("MovieRamaDbContext")
            .Options;
         

            _dbContext = new MovieRamaDbContext(options);
            EmptyDb();
            _dbContext.Users.Add(new User { Id = 1, UserName = "Test User 1" });
            _dbContext.Users.Add(new User { Id = 2, UserName = "Test User 2" });
            _dbContext.SaveChanges();

            _MovieReposut = new MovieRepository(_dbContext);
            _ReactionRepoSut = new ReactionRepository(_dbContext);
        }


        private void EmptyDb()
        {
            if (_dbContext.Users.Any())
            {
                _dbContext.RemoveRange(_dbContext.Users);
                _dbContext.SaveChanges();
            }

            if (_dbContext.Movies.Any())
            {
                _dbContext.RemoveRange(_dbContext.MovieReactions);
                _dbContext.SaveChanges();
            }

            if (_dbContext.Movies.Any())
            {
                _dbContext.RemoveRange(_dbContext.Movies);
                _dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task AddMovieAsync_Should_Add_A_Movie()
        {
            //Arrange
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);
            
            
            //Act
            var actual = _dbContext.Movies.FirstOrDefault(m => m.Id == movieId);


            //Assrt
            movieId.Should().BeGreaterThan(0);

            actual.Should().NotBeNull();
            actual.CreatorId.Should().Be(aMovie.Creator.Id);
            actual.Title.Should().Be(aMovie.Title);
            actual.Description.Should().Be(aMovie.Description);
            actual.CreatorId.Should().Be(aMovie.Creator.Id);
            actual.CreatedAt.Should().Be(aMovie.PublishedAt);
        }

        [Fact]
        public async Task GetMoviesAsync_Return_Movies()
        {
            //Arrange
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);

            //Act
            await _MovieReposut.AddMovieAsync(aMovie);
            await _MovieReposut.AddMovieAsync(aMovie);
            
            var actual = await _MovieReposut.GetMoviesAsync(new MovieRamaWeb.Application.Requests.MovieListQueryParameters());

            actual.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetMoviesAsync_Return_Movies_With_Associated_Reactions()
        {
            //Arrange
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);

            //Act
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);
            var aReaction = Reaction.Create(2, movieId, Domain.Enums.PreferenceType.Like);
            await _ReactionRepoSut.AddReactionAsync(aReaction);
            
            var actual = await _MovieReposut.GetMoviesAsync(new MovieRamaWeb.Application.Requests.MovieListQueryParameters());
            var actualMovie = actual.FirstOrDefault();

            actualMovie.Should().NotBeNull();
            actualMovie.NumberOfLikes.Should().Be(1);
            
        }

        [Fact]
        public async Task GetMovieByIdAsync_Fetches_A_Movie()
        {
            //Arrange
            var userId = 1;
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(userId, ""), DateTime.UtcNow);

            //Act
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);
            var actual = await _MovieReposut.GetMovieByIdAsync(movieId);

            actual.Should().NotBeNull();
            actual.Creator.Id.Should().Be(userId);
            
        }

        [Fact]
        public async Task GetMovieByIdAsync_Returns_Null_When_Movie_Not_Exists ()
        {
            //Arrange
            var userId = 1;
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(userId, ""), DateTime.UtcNow);

            //Act
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);
            var actual = await _MovieReposut.GetMovieByIdAsync(0);

            actual.Should().BeNull();

        }
    }
}
