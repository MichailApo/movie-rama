using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRamaWeb.Domain;
using MovieRamaWeb.Domain.Enums;
using MovieRamaWeb.Application.Requests;
using MovieRamaWeb.Application.Services;

namespace MovieRamaWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMovieRepository _movieRepository;

        /// <summary>
        /// Data to Display
        /// </summary>
        public IEnumerable<Movie> Movies { get; private set; } = Enumerable.Empty<Movie>();

        /// <summary>
        /// Ordering of the page
        /// </summary>
        public SortOrder SortOrder { get; private set; }

        /// <summary>
        /// Sorting Type of the page
        /// </summary>
        public MovieSortType? SortType { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        public async Task OnGetAsync([FromQuery] MovieListQueryParameters queryParameters, [FromRoute] int? id = null)
        {
            SortOrder = queryParameters.SortOrder;
            SortType = queryParameters.SortType;
            Movies = id.HasValue
                ? await _movieRepository.GetMoviesByCreatorIdAsync(id.Value, queryParameters)
                : await _movieRepository.GetMoviesAsync(queryParameters);
        }
    }
}