using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRamaWeb.Domain;
using MovieRamaWeb.Domain.Enums;
using MovieRamaWeb.Application.Requests;
using MovieRamaWeb.Application.Services;
using Application.Services;
using MovieRamaWeb.ViewModels;
using Domain.Enums;

namespace MovieRamaWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IReactionService _reactionService;

        /// <summary>
        /// Data to Display
        /// </summary>
        public IEnumerable<MovieViewModel> Movies { get; private set; } = Enumerable.Empty<MovieViewModel>();

        /// <summary>
        /// Ordering of the page
        /// </summary>
        public SortOrder SortOrder { get; private set; }

        /// <summary>
        /// Sorting Type of the page
        /// </summary>
        public MovieSortType? SortType { get; private set; }

        public int? SelectedCreatorId { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IMovieRepository movieRepository , IReactionService reactionService)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _reactionService = reactionService;
        }

        public async Task OnGetAsync([FromQuery] MovieListQueryParameters queryParameters, [FromRoute] int? id = null)
        {
            SortOrder = queryParameters.SortOrder == SortOrder.Asc 
                ? SortOrder.Desc 
                : SortOrder.Asc;
            SortType = queryParameters.SortType;
            SelectedCreatorId = id;
            
            var movies = id.HasValue
                ? await _movieRepository.GetMoviesByCreatorIdAsync(id.Value, queryParameters)
                : await _movieRepository.GetMoviesAsync(queryParameters);
            
            var userReactions = await _reactionService.GetUserReactionsAsync(User);
            
            Movies = movies.Select(f => {
                
                return userReactions.TryGetValue(f.Id, out var pref) 
                ? new MovieViewModel(f, pref) 
                : new MovieViewModel(f, null);
                }
            );
        }
    }
}