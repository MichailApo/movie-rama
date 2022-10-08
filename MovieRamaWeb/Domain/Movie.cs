namespace MovieRamaWeb.Domain
{
    public class Movie
    {
        public string Title { get; }
        public string Description { get; }
        public User Creator { get; }
        public int NumberOfLikes { get; set; }
        public int NumberOfHates { get; set; }

        public static Movie Create(string title, string description, User creator) => new Movie(title, description, creator);

        private Movie(string title, string description, User creator)
        {
            Title = title;
            Description = description;
            Creator = creator;
        }
    }
}
