using Microsoft.AspNetCore.SignalR;
using WatchMatchApi.Exceptions;
using WatchMatchApi.Models;
using WatchMatchApi.Services;

namespace WatchMatchApi.Hubs
{
    public interface IRoomClient
    {
        Task ReceiveMessage(string message);
        Task ReceiveError(string message);
    }

    public class RoomHub(RoomService roomService) : Hub<IRoomClient>
    {
        private readonly RoomService _roomService = roomService;

        private string? RoomId => Context.GetHttpContext()?.Request?.Query["roomId"].ToString();
        private string? UserId => Context.UserIdentifier;

        public override async Task OnConnectedAsync()
        {
            if (UserId == null) 
            {
                AbortConnection("No user ID.");
                return;
            }
            if (RoomId == null)
            {
                AbortConnection("No room ID.");
                return;
            }

            Room? room;
            try
            {
                room = _roomService.TryGetSignRoom(RoomId, UserId);
            }
            catch (RoomNotFoundException)
            {
                AbortConnection("Room not found.");
                return;
            }

            if (room == null)
            {
                AbortConnection("No access to room.");
                return;
            }

            try
            {
                _roomService.ConnectUserToRoom(Context.ConnectionId, RoomId);
            }
            catch 
            {
                AbortConnection("Try later.");
                return;
            }

            await Clients.Caller.ReceiveMessage(room.Id);
            await base.OnConnectedAsync();
        }

        private async void AbortConnection(string message)
        {
            await Clients.Caller.ReceiveError(message);
            Context.Abort();
            return;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (RoomId != null)
            {
                _roomService.DisconnectUserFromRoom(Context.ConnectionId, RoomId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
