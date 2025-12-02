using Microsoft.AspNetCore.Mvc;
using TMDbLib.Objects.Movies;
using WatchMatchApi.Exceptions;
using WatchMatchApi.Services;

namespace WatchMatchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(RoomService roomService, ILogger<RoomController> logger, MovieService movieService) : ControllerBase
    {
        private readonly RoomService _roomService = roomService;
        private readonly ILogger<RoomController> _logger = logger;
        private readonly MovieService _movieService = movieService;

        [HttpGet]
        public async Task<List<Movie>> Get() 
        {
            var movies = await _movieService.DiscoverRandomMoviesAsync();
            return movies;
        }

        [HttpPatch("{roomId}/users")]
        public IActionResult Patch(string roomId, [FromBody] string userId)
        {
            try
            {
                var room = _roomService.TryGetSignRoom(roomId, userId);

                if (room == null)
                {
                    return StatusCode(403); // Forbid
                }
            }
            catch (RoomNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] string creatorId)
        {
            try
            {
                var room = _roomService.CreateRoom(creatorId);
                if (room != null)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("{roomId} created by {creatorId}", room.Id, creatorId);
                    }
                    return Ok(room.Id);
                }
            }
            catch {}
            return BadRequest();
        }
    }
}
