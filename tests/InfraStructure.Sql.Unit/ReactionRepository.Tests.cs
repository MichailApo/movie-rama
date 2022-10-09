using Bogus;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Sql;
using Infrastructure.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieRamaWeb.Data;

namespace InfraStructure.Sql.Unit
{
    public class ReactionRepository_Tests
    {
        private ReactionRepository _sut;
        private Mock<MovieRamaDbContext> _dbContextMock;
        public ReactionRepository_Tests()
        {
            _dbContextMock = new Mock<MovieRamaDbContext>(new FakeDbContextOptions());
            _sut = new ReactionRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetUserReactionsAsync_Filters_Only_Users_Active_Reactions()
        {
            var userId = 1;
            var expectedCount = 2;

            var noiseFaker = new Faker<MovieReactionEntity>()
                .Rules((f, m) =>
            {
                m.Active = f.Random.Bool();
                m.MovieId = f.Random.Int();
                m.UserId = userId + 1;
                m.Preference = f.PickRandom<PreferenceType>();
            });

            var expectedFaker = new Faker<MovieReactionEntity>()
                .Rules((f, m) =>
                {
                    m.Active = true;
                    m.MovieId = f.Random.Int();
                    m.UserId = userId;
                    m.Preference = f.PickRandom<PreferenceType>();
                });

            var fakes = noiseFaker.Generate(10);

            fakes.AddRange(expectedFaker.Generate(expectedCount));

            var moviesMockSet = GetMockSet(fakes.AsAsyncQueryable());

            _dbContextMock
                .Setup(m => m.MovieReactions)
                .Returns(moviesMockSet.Object);

            var actual = await _sut.GetUserReactionsAsync(userId);

            actual.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task GetUserReactionsAsync_On_Exception_Throws()
        {
            Action action = () => _dbContextMock
                .Setup(m => m.MovieReactions).Throws<Exception>();

            await _sut.Awaiting(f => f.GetUserReactionsAsync(1)).Should().ThrowAsync<Exception>();

        }

        private Mock<DbSet<MovieReactionEntity>> GetMockSet(IQueryable<MovieReactionEntity> fakeSet)
        {
            var moviesMockSet = new Mock<DbSet<MovieReactionEntity>>();
            moviesMockSet.As<IQueryable<MovieReactionEntity>>().Setup(m => m.Provider).Returns(fakeSet.Provider);
            moviesMockSet.As<IQueryable<MovieReactionEntity>>().Setup(m => m.Expression).Returns(fakeSet.Expression);
            moviesMockSet.As<IQueryable<MovieReactionEntity>>().Setup(m => m.ElementType).Returns(fakeSet.ElementType);
            moviesMockSet.As<IQueryable<MovieReactionEntity>>().Setup(m => m.GetEnumerator()).Returns(fakeSet.GetEnumerator());

            return moviesMockSet;
        }
    }
}