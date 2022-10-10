using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MovieRamaWeb.Application.Services;
using MovieRamaWeb.Domain;
using System.Security.Claims;

namespace Application.Tests.Unit
{
    public class ReactionServiceTests
    {
        private readonly ReactionService _sut;
        private readonly Mock<IReactionRepository> _reactionRepoMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IMovieRepository> _movieRepository;
        public ReactionServiceTests()
        {
            _reactionRepoMock = new Mock<IReactionRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _movieRepository = new Mock<IMovieRepository>();
            _sut = new ReactionService(new NullLogger<ReactionService>(), _reactionRepoMock.Object, _authServiceMock.Object, _movieRepository.Object);
        }

        [Fact]
        public async Task GetUserReactionsAsync_Returns_Empty_On_Not_Auth_User()
        {
            //Arrange
            _authServiceMock
                .Setup(f => f.GetUser(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => null);

            //Act
            var actual = await _sut.GetUserReactionsAsync(new ClaimsPrincipal());

            //Assert
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();

        }

        [Fact]
        public async Task GetUserReactionsAsync_HandlesExceptions_Gracefully()
        {
            //Arrange
            _authServiceMock
                .Setup(f => f.GetUser(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => new MovieRamaWeb.Domain.User(1, ""));

            _reactionRepoMock
                .Setup(f => f.GetUserReactionsAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            //Act
            var actual = await _sut.GetUserReactionsAsync(new ClaimsPrincipal());

            //Assert
            actual.Should().NotBeNull();
            actual.Should().BeEmpty();

        }

        [Fact]
        public async Task GetUserReactionsAsync_Returns_Results()
        {
            //Arrange
            _authServiceMock
                .Setup(f => f.GetUser(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => new MovieRamaWeb.Domain.User(1, ""));

            _reactionRepoMock
                .Setup(f => f.GetUserReactionsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Reaction[]
                {
                    Reaction.Create(1,1,Domain.Enums.PreferenceType.Like),
                    Reaction.Create(1,2,Domain.Enums.PreferenceType.Like)
                });

            //Act
            var actual = await _sut.GetUserReactionsAsync(new ClaimsPrincipal());

            //Assert
            actual.Should().NotBeNull();
            actual.Should().NotBeEmpty();
            actual.Should().HaveCount(2);

        }

        [Fact]
        public async Task AddReactionAsync_ThrowsException_When_Movie_Not_Exists()
        {
            var reaction = Reaction.Create(1, 1, Domain.Enums.PreferenceType.Like);
            _movieRepository
                .Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            //Act
            Func<Task> act = () => _sut.AddReactionAsync(reaction);

            await act.Should().ThrowExactlyAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddReactionAsync_ThrowsException_A_User_Reacts_To_His_Movie()
        {
            var userId = 1;
            var reaction = Reaction.Create(userId, 1, Domain.Enums.PreferenceType.Like);
            _movieRepository
                .Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => Movie.Create(1, "", "", new User(userId, ""), DateTime.UtcNow, 0, 0));
            //Act
            Func<Task> act = () => _sut.AddReactionAsync(reaction);

            await act.Should().ThrowExactlyAsync<ValidationException>();
        }
    }
}