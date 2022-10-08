namespace MovieRamaWeb.Domain
{
    public class Movie
    {
        public string Title { get; }
        public string Description { get; }
        public User Creator { get; }
        public DateTime PublishedAt { get; }
        public int NumberOfLikes { get; set; }
        public int NumberOfHates { get; set; }

        public static Movie Create(string title, string description, User creator, DateTime publishedAt) => new(title, description, creator, publishedAt);

        private Movie(string title, string description, User creator, DateTime publishedAt)
        {
            Title = title;
            Description = description;
            Creator = creator;
            PublishedAt = publishedAt;
        }
    }
}
