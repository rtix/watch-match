using Microsoft.AspNetCore.Mvc;
using WatchMatchApi.Services;

namespace WatchMatchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(RoomService roomService, ILogger<RoomController> logger) : ControllerBase
    {
        private readonly RoomService _roomService = roomService;
        private readonly ILogger<RoomController> _logger = logger;
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
