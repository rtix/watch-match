using System.Collections.Concurrent;
using WatchMatchApi.Models;

namespace WatchMatchApi.Services
{
    public class RoomService
    {
        private readonly Random _rand = new();
        private readonly ILogger<RoomService> _logger;

        private readonly ConcurrentDictionary<string, Room> _rooms = new();
        private readonly GroupTrackerService _groupTrackerService;

        private Room? GetRoom(string roomId) => _rooms.TryGetValue(roomId, out var room) ? room : null;
        public bool RoomExists(string roomId) => _rooms.ContainsKey(roomId);

        public RoomService(GroupTrackerService groupTrackerService, ILogger<RoomService> logger)
        {
            _logger = logger;
            _groupTrackerService = groupTrackerService;

            _groupTrackerService.GroupDeleted += groupId => RemoveRoom(groupId);
        }

        public void DisconnectUserFromRoom(string userId, string roomId)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{userId} disconnected from {roomId}", userId, roomId);
            }
            _groupTrackerService.RemoveUserFromGroup(roomId, userId);
        }

        public void ConnectUserToRoom(string userId, string roomId)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{userId} connected to {roomId}", userId, roomId);
            }
            if (!_groupTrackerService.AddUserToGroup(roomId, userId))
            {
                throw new Exception("No group.");
            }
        }

        private string GenerateRoomCode()
        {
            if (_rooms.Count > 5000)
            {
                throw new Exception("Too many rooms.");
            }

            string code;
            do
            {
                code = _rand.Next(0, 10000).ToString("D4");
            }
            while (_rooms.ContainsKey(code));

            return code;
        }

        public Room? CreateRoom(string creatorId)
        {
            string roomCode = GenerateRoomCode();
            Room room = new() { CreatorId = creatorId, Id = roomCode };
            bool ok = _rooms.TryAdd(roomCode, room);
            if (ok)
            {
                _groupTrackerService.CreateGroup(roomCode);
                return room;
            }
            return null;
        }

        public Room? GetOrSignRoomIfAvailable(string roomId, string guestId)
        {
            Room? room = GetRoom(roomId);

            if (room == null)
            {
                return null;
            }

            if (room.CreatorId != guestId)
            {
                lock (room)
                {
                    room.GuestId ??= guestId;
                }

                if (room.GuestId != guestId)
                {
                    return null;
                }
            }

            return room;
        }

        public bool RemoveRoom(string roomId)
        {
            return _rooms.TryRemove(roomId, out _);
        }
    }
}
