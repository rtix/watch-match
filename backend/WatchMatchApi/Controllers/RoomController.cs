using Microsoft.AspNetCore.Mvc;
using TMDbLib.Objects.Search;
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
        public async Task<List<SearchMovie>> Get() 
        {
            var movies = await _movieService.DiscoverRandomMovies();
            return movies;
        }

        [HttpPost]
        public object Post([FromBody] string creatorId)
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
                    return Ok(room);
                }
            }
            catch {}
            return BadRequest();
        }
    }
}
