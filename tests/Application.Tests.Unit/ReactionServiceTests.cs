using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MovieRamaWeb.Application.Services;
using System.Security.Claims;

namespace Application.Tests.Unit
{
    public class ReactionServiceTests
    {
        private readonly ReactionService _sut;
        private readonly Mock<IReactionRepository> _reactionRepoMock;
        private readonly Mock<IAuthService> _authServiceMock
            ;
        public ReactionServiceTests()
        {
            _reactionRepoMock = new Mock<IReactionRepository>();
            _authServiceMock = new Mock<IAuthService>();
            _sut = new ReactionService(new NullLogger<ReactionService>(),_reactionRepoMock.Object, _authServiceMock.Object);
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
                .Returns(() => new MovieRamaWeb.Domain.User(1,""));

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
    }
}