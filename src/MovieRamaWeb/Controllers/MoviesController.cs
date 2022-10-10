using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieRamaWeb.Controllers
{
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        public MoviesController()
        {
        }

        [HttpPost("/movie/{id}/like")]
        public async Task<IActionResult> LikeMovieAsync([FromRoute]int id)
        {

            return Ok();
        }

        [HttpPost("/movie/{id}/hate")]
        public async Task<IActionResult> HateMovieAsync([FromRoute] int id)
        {

            return Ok();
        }
    }
}
