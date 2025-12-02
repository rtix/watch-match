using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace WatchMatchApi.Models
{
    public class RoomUserData
    {
        public bool IsCreator { get; init; } = false;
    }

    public class Room
    {
        private readonly ConcurrentDictionary<string, RoomUserData> _users = new();
        private readonly int _capacity;

        [RegularExpression("^[a-zA-Z0-9]{4}$", ErrorMessage = "Invalid room id")]
        public string Id { get; init; }

        public Room(string roomId, string creatorId, int capacity = 2)
        {
            Id = roomId;
            _capacity = capacity;
            _users.TryAdd(creatorId, new RoomUserData { IsCreator = true });
        }

        public bool TryAddUser(string userId)
        {
            var user = new RoomUserData();
            return _users.TryAdd(userId, user);
        }

        public bool ContainsUser(string userId) => _users.ContainsKey(userId);
        public bool IsFull => _users.Count >= _capacity;
    }
}
