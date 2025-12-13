using Microsoft.AspNetCore.SignalR;
using TMDbLib.Objects.Movies;
using WatchMatchApi.DTOs;
using WatchMatchApi.Exceptions;
using WatchMatchApi.Models;
using WatchMatchApi.Services;

namespace WatchMatchApi.Hubs
{
    public static class RoomHubMessages
    {
        public const string RoomNotFound = "Room {0} not found";
        public const string NoAccessToRoom = "No access to room";
        public const string TryLater = "Try later";
    }

    public interface IRoomClient
    {
        Task ReceiveMessage(string message);
        Task ReceiveError(string message);

        Task RecieveMovies(List<Movie> movies);
        Task RecieveLikes(List<MovieLikesDto> likes);
    }

    public class RoomHub(RoomService roomService, MovieService movieService) : Hub<IRoomClient>
    {
        private readonly RoomService _roomService = roomService;
        private readonly MovieService _movieService = movieService;

        private string RoomId => (string) (Context.Items["RoomId"] ?? throw new MissingRoomIdException());
        private string UserId => (string) (Context.Items["UserId"] ?? throw new MissingUserIdException());
        private string ConnectionId => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            Room? room;
            try
            {
                room = _roomService.TryGetSignRoom(RoomId, UserId);
            }
            catch (RoomNotFoundException)
            {
                AbortConnection(string.Format(RoomHubMessages.RoomNotFound, RoomId));
                return;
            }
            if (room == null)
            {
                AbortConnection(RoomHubMessages.NoAccessToRoom);
                return;
            }

            try
            {
                _roomService.ConnectUserToRoom(ConnectionId, RoomId);
            }
            catch 
            {
                AbortConnection(RoomHubMessages.TryLater);
                return;
            }

            await Groups.AddToGroupAsync(ConnectionId, RoomId);
            await Clients.Caller.RecieveLikes(await GetFullMoviesByLikes(room));
            await base.OnConnectedAsync();
        }

        private async Task<List<MovieLikesDto>> GetFullMoviesByLikes(Room room)
        {
            var tasks = room.GetMoviesByLikes().Select(async x =>
            {
                var result = await _movieService.GetMovieAsync(x.MovieId);
                return new MovieLikesDto { Movie = result, Likes = x.LikeCount };
            });

            return (await Task.WhenAll(tasks)).ToList();
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
                _roomService.DisconnectUserFromRoom(ConnectionId, RoomId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async void SendLike(int movieId)
        {
            var room = _roomService.TryGetSignRoom(RoomId, UserId);
            if (room == null)
            {
                AbortConnection(string.Format(RoomHubMessages.RoomNotFound, RoomId));
                return;
            }
            room.LikeMovie(UserId, movieId);
            await Clients.Group(RoomId).RecieveLikes(await GetFullMoviesByLikes(room));
        }

        public async void SendDislike(int movieId)
        {
            var room = _roomService.TryGetSignRoom(RoomId, UserId);
            if (room == null)
            {
                AbortConnection(string.Format(RoomHubMessages.RoomNotFound, RoomId));
                return;
            }
            room.ReactToMovie(UserId, movieId);
        }

        public async Task<List<MovieDto>> RequestMovies()
        {
            var room = _roomService.TryGetSignRoom(RoomId, UserId);
            if (room == null)
            {
                AbortConnection(string.Format(RoomHubMessages.RoomNotFound, RoomId));
                return new();
            }
            var debtMovieIds = room.GetUserDebt(UserId);
            var debtMovies = debtMovieIds.Select(async id => await _movieService.GetMovieAsync(id));
            var discoverSize = Math.Max(0, MovieService.DEFAULT_DISCOVER_SIZE - debtMovieIds.Count);
            var randomMovies = discoverSize == 0 ? [] : await _movieService.DiscoverRandomMoviesAsync(discoverSize);
            var result = (await Task.WhenAll(debtMovies)).ToList();
            result.AddRange(randomMovies);
            return result;
        }
    }
}
