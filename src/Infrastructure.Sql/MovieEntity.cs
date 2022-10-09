using Infrastructure.Sql;

namespace MovieRamaWeb.Data
{
    public class MovieEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Creator { get; set; }
        public int CreatorId { get; set; }
        public ICollection<MovieReactionEntity> Reactions { get; set; } = new List<MovieReactionEntity>();
    }
}
