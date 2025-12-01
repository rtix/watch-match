namespace WatchMatchApi.Exceptions
{
    public class RoomNotFoundException(string roomId) : Exception($"Room {roomId} does not exist")
    {
    }
}
