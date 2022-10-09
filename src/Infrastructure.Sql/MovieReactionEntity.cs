using Domain.Enums;
using MovieRamaWeb.Data;

namespace Infrastructure.Sql
{
    /// <summary>
    /// Represents a preference relationship between <see cref="MovieEntity"/> and <see cref="MovieRamaWeb.Data.User"/>
    /// </summary>
    public class MovieReactionEntity
    {
        public int MovieId { get; set; }
        public MovieEntity Movie { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool Active { get; set; }
        public PreferenceType Preference { get; set; }
    }
}
