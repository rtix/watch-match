using Microsoft.AspNetCore.SignalR;

namespace WatchMatchApi.Exceptions
{
    public class MissingRoomIdException() : HubException($"Missing RoomId")
    {
    }

    public class MissingUserIdException() : HubException($"Missing UserId")
    {
    }
}
