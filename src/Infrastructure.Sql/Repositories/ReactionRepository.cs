using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MovieRamaWeb.Data;

namespace Infrastructure.Sql.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly MovieRamaDbContext _dbContext;

        public ReactionRepository(MovieRamaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReactionAsync(Reaction reaction)
        {

            var existing = _dbContext.MovieReactions.FirstOrDefault(f =>
            f.MovieId == reaction.MovieId
            && f.UserId == reaction.UserId);

            if (existing == null)
            {
                var entity = new MovieReactionEntity
                {
                    Active = true,
                    MovieId = reaction.MovieId,
                    UserId = reaction.UserId,
                    Preference = reaction.Preference
                };
                await _dbContext.MovieReactions.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return;
            }

            if (existing.Preference == reaction.Preference && existing.Active)
            {
                //No-op
                return;
            }

            existing.Preference = reaction.Preference;
            existing.Active = true;
            _dbContext.MovieReactions.Update(existing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Reaction>> GetUserReactionsAsync(int userId)
        {
            return await _dbContext.MovieReactions
                .Where(m => m.UserId == userId && m.Active)
                .Select(m => Reaction.Create(m.UserId, m.MovieId, m.Preference))
                .ToListAsync();
        }

        public async Task RemoveReactionAsync(int userId, int movieId)
        {
            var existingActive = _dbContext.MovieReactions
                .FirstOrDefault(m => m.UserId == userId 
                && m.MovieId == movieId
                && m.Active);
            
            if( existingActive != null)
            {
                existingActive.Active = false;
                _dbContext.Update(existingActive);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
