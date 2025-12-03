namespace WatchMatchApi.Exceptions
{
    public class RoomNotFoundException(string roomId) : Exception($"Room {roomId} does not exist")
    {
    }

    public class UserNotInTheRoomException(string userId, string roomId) : Exception($"User {userId} not in the room {roomId}")
    {
    }
}
