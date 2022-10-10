using Domain.Entities;
using Domain.Enums;
using System.Security.Claims;

namespace Application.Services
{
    public interface IReactionService
    {
        Task<IDictionary<int, PreferenceType>> GetUserReactionsAsync(ClaimsPrincipal claimsPrincipal);
        Task AddReactionAsync(Reaction reaction);
        Task RemoveReaction(int userId,int movieId);
    }
}
