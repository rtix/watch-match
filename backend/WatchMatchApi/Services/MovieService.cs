using TMDbLib.Objects.Movies;
using WatchMatchApi.ApiClients;
using WatchMatchApi.DTOs;

namespace WatchMatchApi.Services
{
    public class MovieService(TMDbApi tmdbApi)
    {
        private readonly TMDbApi _api = tmdbApi;
        public const int DEFAULT_DISCOVER_SIZE = 10;

        public async Task<MovieDto> GetMovieAsync(int movieId)
        {
            return MovieMapper.ToDto(await _api.GetMovieAsync(movieId));
        }

        public async Task<List<MovieDto>> DiscoverRandomMoviesAsync(int moviesCount=DEFAULT_DISCOVER_SIZE)
        {
            Movie[] randomMovies = new Movie[moviesCount];
            var maxPages = Math.Min(_api.DiscoverMoviesAsync(1).Result.TotalPages, TMDbApi.MAX_PAGES_LIMIT);
            var random = new Random();

            var tasks = Enumerable.Range(0, moviesCount).Select(async _ =>
            {
                var randomPageNumber = random.Next(1, maxPages + 1);
                var randomPage = _api.DiscoverMoviesAsync(randomPageNumber);
                var numberOfItemsInPage = randomPage.Result.Results.Count;
                var randomItemNumber = random.Next(0, numberOfItemsInPage);
                var randomItem = randomPage.Result.Results[randomItemNumber];
                var fullMovieData = await _api.GetMovieAsync(randomItem.Id, MovieMethods.Videos | MovieMethods.Images);
                return MovieMapper.ToDto(fullMovieData);
            });

            return (await Task.WhenAll(tasks)).ToList();
        }
    }
}
