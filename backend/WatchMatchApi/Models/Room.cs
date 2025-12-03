using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using WatchMatchApi.Exceptions;

namespace WatchMatchApi.Models
{
    public class RoomUserData
    {
        public bool IsCreator { get; init; } = false;
        public ConcurrentDictionary<int, byte> ReactionDebt { get; } = new();
    }

    public class Room
    {
        private readonly ConcurrentDictionary<string, RoomUserData> _users = new();
        private readonly int _capacity;

        private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, byte>> _likedMovies = new();

        private List<(int MovieId, int LikeCount)> _moviesByLikes = new();
        private readonly object _popularityLock = new();

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
            if (_users.TryAdd(userId, user))
            {
                foreach (var movieId in _likedMovies.Keys)
                {
                    user.ReactionDebt.TryAdd(movieId, 0);
                }
                return true;
            }
            return false;
        }

        public bool ContainsUser(string userId) => _users.ContainsKey(userId);
        public bool IsFull => _users.Count >= _capacity;

        public void LikeMovie(string userId, int movieId)
        {
            if (!_users.ContainsKey(userId))
                throw new UserNotInTheRoomException(userId, Id);

            var userSet = _likedMovies.GetOrAdd(movieId, _ =>
            {
                foreach (var user in _users.Values)
                    user.ReactionDebt.TryAdd(movieId, 0);

                return new ConcurrentDictionary<string, byte>();
            });

            if (userSet.TryAdd(userId, 0))
                UpdateMoviesByLikes();

            ReactToMovie(userId, movieId);
        }

        public void ReactToMovie(string userId, int movieId)
        {
            if (!_users.ContainsKey(userId))
                throw new UserNotInTheRoomException(userId, Id);

            _users[userId].ReactionDebt.TryRemove(movieId, out _);
        }

        private void UpdateMoviesByLikes()
        {
            lock (_popularityLock)
            {
                _moviesByLikes = _likedMovies
                    .Select(kvp => (kvp.Key, kvp.Value.Count))
                    .OrderByDescending(x => x.Count)
                    .ToList();
            }
        }

        public List<(int MovieId, int LikeCount)> GetMoviesByLikes()
        {
            lock (_popularityLock)
            {
                return _moviesByLikes.ToList();
            }
        }

        public List<string> GetUsersWhoLiked(int movieId)
        {
            if (_likedMovies.TryGetValue(movieId, out var userSet))
                return userSet.Keys.ToList();
            return new List<string>();
        }

        public List<int> GetUserDebt(string userId, int limit=10)
        {
            return _users[userId].ReactionDebt.Keys.Take(limit).ToList();
        }
    }
}
