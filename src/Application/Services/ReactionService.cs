using Domain.Enums;
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

        public ReactionService(ILogger<ReactionService> logger, IReactionRepository reactionRepository, IAuthService authService)
        {
            _logger = logger;
            _reactionRepository = reactionRepository;
            _authService = authService;
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
    }
}
