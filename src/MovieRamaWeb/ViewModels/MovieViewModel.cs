using Domain.Enums;
using MovieRamaWeb.Domain;

namespace MovieRamaWeb.ViewModels
{
    public class MovieViewModel
    {
        private readonly Movie _movie;
        private readonly PreferenceType? _userPrefence;

        public MovieViewModel(Movie movie, PreferenceType? userPrefence)
        {
            _movie = movie;
            _userPrefence = userPrefence;
        }

        public int Id => _movie.Id;
        public string Title => _movie.Title;
        public string Description => _movie.Description;
        public User Creator => _movie.Creator;
        public DateTime PublishedAt => _movie.PublishedAt;
        public int NumberOfLikes => _movie.NumberOfLikes;
        public int NumberOfHates => _movie.NumberOfHates;

        public string PreferenceText =>
            _userPrefence switch
            {
                PreferenceType.Hate => "You hate this movie",
                PreferenceType.Like => "You Like this movie",
                _ => string.Empty
            };

    }
}
