using Bogus;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Sql;
using Moq;
using MovieRamaWeb.Application.Requests;
using MovieRamaWeb.Data;
using MovieRamaWeb.Data.Repositories;
using User = MovieRamaWeb.Data.User;

namespace InfraStructure.Sql.Unit
{
    public class MovieRepository_Tests
    {
        private MovieRepository _sut;
        private Mock<MovieRamaDbContext> _dbContextMock;
        public MovieRepository_Tests()
        {
            _dbContextMock = new Mock<MovieRamaDbContext>(new FakeDbContextOptions());
            _sut = new MovieRepository(_dbContextMock.Object);
        }


        [Theory]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Asc)]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Desc)]
        public async Task GetMoviesAsync_Results_Are_Ordered_On_Publish_Date(MovieRamaWeb.Domain.Enums.SortOrder sortOrder)
        {
            var fakes = GetFakes(10);
            var moviesMockSet = DbSetMocker<MovieEntity>.GetMockSet(fakes.AsAsyncQueryable());

            _dbContextMock
                .Setup(m => m.Movies)
                .Returns(moviesMockSet.Object);

            var parameters = new MovieListQueryParameters()
            {
                SortOrder = sortOrder,
                SortType = MovieRamaWeb.Domain.Enums.MovieSortType.Date
            };

            var actual = await _sut.GetMoviesAsync(parameters);

            //Assert
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Asc)
            {
                actual.Should().BeInAscendingOrder(f => f.PublishedAt);
            }
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Desc)
            {
                actual.Should().BeInDescendingOrder(f => f.PublishedAt);
            }

        }

        [Theory]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Asc)]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Desc)]
        public async Task GetMoviesAsync_Results_Are_Ordered_On_NumberOfHates(MovieRamaWeb.Domain.Enums.SortOrder sortOrder)
        {
            var fakes = GetFakes(10);
            var moviesMockSet = DbSetMocker<MovieEntity>.GetMockSet(fakes.AsAsyncQueryable());

            _dbContextMock
                .Setup(m => m.Movies)
                .Returns(moviesMockSet.Object);

            var parameters = new MovieListQueryParameters()
            {
                SortOrder = sortOrder,
                SortType = MovieRamaWeb.Domain.Enums.MovieSortType.Hates
            };

            var actual = await _sut.GetMoviesAsync(parameters);

            //Assert
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Asc)
            {
                actual.Should().BeInAscendingOrder(f => f.NumberOfHates);
            }
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Desc)
            {
                actual.Should().BeInDescendingOrder(f => f.NumberOfHates);
            }

        }

        [Theory]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Asc)]
        [InlineData(MovieRamaWeb.Domain.Enums.SortOrder.Desc)]
        public async Task GetMoviesAsync_Results_Are_Ordered_On_NumberOfLikes(MovieRamaWeb.Domain.Enums.SortOrder sortOrder)
        {
            var fakes = GetFakes(10);
            var moviesMockSet = DbSetMocker<MovieEntity>.GetMockSet(fakes.AsAsyncQueryable());

            _dbContextMock
                .Setup(m => m.Movies)
                .Returns(moviesMockSet.Object);

            var parameters = new MovieListQueryParameters()
            {
                SortOrder = sortOrder,
                SortType = MovieRamaWeb.Domain.Enums.MovieSortType.Likes
            };

            var actual = await _sut.GetMoviesAsync(parameters);

            //Assert
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Asc)
            {
                actual.Should().BeInAscendingOrder(f => f.NumberOfLikes);
                
            }
            if (sortOrder == MovieRamaWeb.Domain.Enums.SortOrder.Desc)
            {
                actual.Should().BeInDescendingOrder(f => f.NumberOfLikes);
            }

        }

        [Fact]
        public async Task GetMoviesByCreatorIdAsync_Should_Only_Contain_Movies_Of_Same_Creator()
        {
            var users = new int[] { 1, 2, 3, 4, 5 };
            var userId = new Faker().PickRandom(users);
            var fakes = GetFakes(10, users, userId);
            var moviesMockSet = DbSetMocker<MovieEntity>.GetMockSet(fakes.AsAsyncQueryable());

            _dbContextMock
                .Setup(m => m.Movies)
                .Returns(moviesMockSet.Object);

            var parameters = new MovieListQueryParameters();

            var actual = await _sut.GetMoviesByCreatorIdAsync(userId, parameters);

            actual.Should().OnlyContain(f => f.Creator.Id == userId);
        }


        private List<MovieEntity> GetFakes(int count, int[] userIds,int? creatorIdInput = null)
        {
            
            var movieIds = new int[] { 10, 20, 30, 40, 50 };

            var reactionsFaker = new Faker<MovieReactionEntity>()
                .Rules((f, m) =>
                {
                    m.Active = f.Random.Bool();
                    m.MovieId = f.PickRandom(movieIds);
                    m.UserId = f.PickRandom(userIds);
                    m.Preference = f.PickRandom<PreferenceType>();
                });

            var faker = new Faker<MovieEntity>().Rules((f, m) =>
            {
                var creatorId = creatorIdInput ?? f.PickRandom(userIds);
                m.CreatorId = creatorId;
                m.Reactions = reactionsFaker.GenerateBetween(0, 10);
                m.CreatedAt = f.Date.Past();
                m.Creator = new User()
                {
                    Id = creatorId,
                };

            });

            return faker.Generate(count);
        }
        private List<MovieEntity> GetFakes(int count)
        {
            return GetFakes(count, new int[] { 1, 2, 3, 4, 5 });
        }
    }
}
