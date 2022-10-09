namespace MovieRamaWeb.Domain
{
    public class Movie
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public User Creator { get; }
        public DateTime PublishedAt { get; }
        public int NumberOfLikes { get; }
        public int NumberOfHates { get; }

        /// <summary>
        /// Instatiation method when creating a New <see cref="Movie"/>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="creator"></param>
        /// <param name="publishedAt"></param>
        /// <returns></returns>
        public static Movie CreateNew(string title, string description, User creator, DateTime publishedAt) => new(title, description, creator, publishedAt);

        /// <summary>
        /// Instatiation method when retrieving an existing <see cref="Movie"/>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="creator"></param>
        /// <param name="publishedAt"></param>
        /// <returns></returns>
        public static Movie Create(int id, string title, string description, User creator, DateTime publishedAt, int numberOfLikes, int numberOfHates) => new(id, title, description, creator, publishedAt, numberOfLikes, numberOfHates);

        private Movie(int id, string title, string description, User creator, DateTime publishedAt, int numberOfLikes, int numberOfHates)
        {
            Id = id;
            Title = title;
            Description = description;
            Creator = creator;
            PublishedAt = publishedAt;
            NumberOfHates = numberOfHates;
            NumberOfLikes = numberOfLikes;
        }

        private Movie(string title, string description, User creator, DateTime publishedAt)
        {
            Title = title;
            Description = description;
            Creator = creator;
            PublishedAt = publishedAt;
        }
    }
}
