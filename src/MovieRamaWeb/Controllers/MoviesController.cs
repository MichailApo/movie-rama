using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieRamaWeb.Application.Services;
using MovieRamaWeb.ViewModels;

namespace MovieRamaWeb.Controllers
{
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IAuthService _authService;
        private readonly IReactionService _reactionService;

        public MoviesController(ILogger<MoviesController> logger,IAuthService authService, IReactionService reactionService)
        {
            _logger = logger;
            _authService = authService;
            _reactionService = reactionService;
        }

        [HttpPost("/movie/{id}/like")]
        public async Task<IActionResult> LikeMovieAsync([FromRoute] int id)
        {
            try
            {
                var user = _authService.GetUser(User);

                await _reactionService.AddReactionAsync(Reaction.Create(user.Id, id, PreferenceType.Like));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorMessageModel(ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with submitting a Like Reaction");
                return new StatusCodeResult(500);
            }
            
            
            return Ok();
        }

        [HttpPost("/movie/{id}/hate")]
        public async Task<IActionResult> HateMovieAsync([FromRoute] int id)
        {

            try
            {
                var user = _authService.GetUser(User);

                await _reactionService.AddReactionAsync(Reaction.Create(user.Id, id, PreferenceType.Hate));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorMessageModel(ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with submitting a Hate Reaction");
                return new StatusCodeResult(500);
            }
            return Ok();
        }

        [HttpPost("/movie/{id}/remove-reaction")]
        public async Task<IActionResult> RemoveReactionAsync([FromRoute] int id)
        {

            try
            {
                var user = _authService.GetUser(User);

                await _reactionService.AddReactionAsync(Reaction.Create(user.Id, id, PreferenceType.Hate));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ErrorMessageModel(ex.Message));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ErrorMessageModel(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with submitting a Hate Reaction");
                return new StatusCodeResult(500);
            }
            return Ok();
        }
    }
}
