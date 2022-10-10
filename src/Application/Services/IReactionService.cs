using Domain.Enums;
using System.Security.Claims;

namespace Application.Services
{
    public interface IReactionService
    {
        Task<IDictionary<int, PreferenceType>> GetUserReactionsAsync(ClaimsPrincipal claimsPrincipal);
    }
}
