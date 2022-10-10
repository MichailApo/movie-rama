using Domain.Entities;
using Domain.Enums;
using System.Security.Claims;

namespace Application.Services
{
    public interface IReactionService
    {
        Task<IDictionary<int, PreferenceType>> GetUserReactionsAsync(ClaimsPrincipal claimsPrincipal);
        Task AddReactionAsync(Reaction reaction);
        Task RemoveReactionAsync(int userId,int movieId);
    }
}
