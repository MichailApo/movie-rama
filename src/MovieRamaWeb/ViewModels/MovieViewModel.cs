using Domain.Enums;
using MovieRamaWeb.Domain;

namespace MovieRamaWeb.ViewModels
{
    public class MovieViewModel
    {
        private readonly Movie _movie;
        

        public MovieViewModel(Movie movie, PreferenceType? userPrefence)
        {
            _movie = movie;
            UserPreference = userPrefence;
        }

        public int Id => _movie.Id;
        public string Title => _movie.Title;
        public string Description => _movie.Description;
        public User Creator => _movie.Creator;
        public DateTime PublishedAt => _movie.PublishedAt;
        public int NumberOfLikes => _movie.NumberOfLikes;
        public int NumberOfHates => _movie.NumberOfHates;
        public PreferenceType? UserPreference { get; }
        public string PreferenceText =>
            UserPreference switch
            {
                PreferenceType.Hate => "You hate this movie",
                PreferenceType.Like => "You Like this movie",
                _ => string.Empty
            };

    }
}
