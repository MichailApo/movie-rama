using System.ComponentModel.DataAnnotations;

namespace MovieRamaWeb.Data
{
    public class MovieEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public User Creator { get; set; }
        public int CreatorId { get; set; }
    }
}
