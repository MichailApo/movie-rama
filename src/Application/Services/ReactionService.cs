using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using MovieRamaWeb.Application.Services;
using System.Security.Claims;

namespace Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly ILogger<ReactionService> _logger;
        private readonly IReactionRepository _reactionRepository;
        private readonly IAuthService _authService;
        private readonly IMovieRepository _movieRepository;

        public ReactionService(ILogger<ReactionService> logger,
            IReactionRepository reactionRepository,
            IAuthService authService,
            IMovieRepository movieRepository)
        {
            _logger = logger;
            _reactionRepository = reactionRepository;
            _authService = authService;
            _movieRepository = movieRepository;
        }

        /// <summary>
        /// Returns a user's preference for all of his movies
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public async Task<IDictionary<int, PreferenceType>> GetUserReactionsAsync(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var user = _authService.GetUser(claimsPrincipal);
                if (user == null)
                {
                    return new Dictionary<int, PreferenceType>();
                }
                var userReactions = await _reactionRepository.GetUserReactionsAsync(user.Id);

                return userReactions.ToDictionary(r => r.MovieId, r => r.Preference);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while retrieving user's reactions");
                return new Dictionary<int, PreferenceType>();
            }

        }

        public async Task AddReactionAsync(Reaction reaction)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(reaction.MovieId);
            if (movie == null)
            {
                throw new NotFoundException($"Movie Id {reaction.MovieId} not found");
            }

            if (movie.Creator.Id == reaction.UserId)
            {
                throw new ValidationException("You cannot add preference on your own movies");
            }

            await _reactionRepository.AddReactionAsync(reaction);

        }

        public async Task RemoveReaction(int userId, int movieId)
        {
            await _reactionRepository.RemoveReactionAsync(userId, movieId);
        }
    }
}
