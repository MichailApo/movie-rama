using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRamaWeb.Services;
using MovieRamaWeb.Domain;
using System.ComponentModel.DataAnnotations;

namespace MovieRamaWeb.Pages
{
    [Authorize]
    public class MovieSubmitModel : PageModel
    {
        private readonly ILogger<MovieSubmitModel> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IAuthService _authService;

        [Required]
        [BindProperty]
        public string MovieTitle { get; set; }

        [Required]
        [BindProperty]
        public string MovieDescription { get; set; }

        public MovieSubmitModel(
            ILogger<MovieSubmitModel> logger,
            IMovieRepository movieRepository,
            IAuthService authService)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var user = _authService.GetUser(User);
                var movie = Movie.Create(MovieTitle, MovieDescription, user);
                await _movieRepository.AddMovieAsync(movie);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while submitting a movie");
                ModelState.AddModelError("ErrMsg", "Something went wrong. Try Again Later");
                return Page();
            }


            return Redirect("/");
        }
    }
}
