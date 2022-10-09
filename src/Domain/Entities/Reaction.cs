using Domain.Enums;

namespace Domain.Entities
{
    public class Reaction
    {
        public int UserId { get; }
        public int MovieId { get; }
        public PreferenceType Preference { get; }

        public static Reaction Create(int userId, int movieId, PreferenceType preference) => new(userId, movieId, preference);
        private Reaction(int userId, int movieId, PreferenceType preference)
        {
            UserId = userId;
            MovieId = movieId;
            Preference = preference;
        }
    }
}
