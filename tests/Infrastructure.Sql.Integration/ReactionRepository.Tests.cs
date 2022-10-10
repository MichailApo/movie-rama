using Infrastructure.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Data.Repositories;
using MovieRamaWeb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRamaWeb.Domain;
using Domain.Entities;
using User = MovieRamaWeb.Data.User;
using FluentAssertions;

namespace Infrastructure.Sql.Integration
{
    public class ReactionRepository_Test
    {
        private MovieRamaDbContext _dbContext;
        private MovieRepository _MovieReposut;
        private ReactionRepository _ReactionRepoSut;
        public ReactionRepository_Test()
        {
            var options = new DbContextOptionsBuilder<MovieRamaDbContext>()
            .UseInMemoryDatabase("MovieRamaDbContext")
            .EnableSensitiveDataLogging()
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
        public async Task AddReactionAsync_Adding_The_Same_Preference_Is_Idempotent()
        {
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);

            await _ReactionRepoSut.AddReactionAsync(Reaction.Create(1, movieId, Domain.Enums.PreferenceType.Like));
            await _ReactionRepoSut.AddReactionAsync(Reaction.Create(1, movieId, Domain.Enums.PreferenceType.Like));
            _dbContext.MovieReactions.Should().HaveCount(1);
        }

        [Fact]
        public async Task AddReactionAsync_Adding_A_Reaction_With_Different_Preference_Update_Preference()
        {
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);

            await _ReactionRepoSut.AddReactionAsync(Reaction.Create(1, movieId, Domain.Enums.PreferenceType.Like));
            await _ReactionRepoSut.AddReactionAsync(Reaction.Create(1, movieId, Domain.Enums.PreferenceType.Hate));
            _dbContext.MovieReactions.Should().HaveCount(1);
        }
        
        [Fact]
        public async Task RemoveReactionAsync_Adding_A_Reaction_With_Different_Preference_Update_Preference()
        {
            var aMovie = Movie.CreateNew("asd", "asd", new MovieRamaWeb.Domain.User(1, ""), DateTime.UtcNow);
            var movieId = await _MovieReposut.AddMovieAsync(aMovie);

            await _ReactionRepoSut.AddReactionAsync(Reaction.Create(1, movieId, Domain.Enums.PreferenceType.Like));
            await _ReactionRepoSut.RemoveReactionAsync(1,movieId);
            _dbContext.MovieReactions.Should().HaveCount(1);
            _dbContext.MovieReactions.Count(f => f.Active).Should().Be(0, "Soft deletes reactions");
        }
    }
}
