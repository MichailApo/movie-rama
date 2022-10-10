using Domain.Entities;

namespace Application.Services
{
    public interface IReactionRepository
    {
        Task AddReactionAsync(Reaction reaction);

        Task<IEnumerable<Reaction>> GetUserReactionsAsync(int userId);

        Task RemoveReactionAsync(int userId, int movieId);
    }
}
