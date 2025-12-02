using Microsoft.Extensions.Caching.Memory;
using TMDbLib.Client;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using WatchMatchApi.Exceptions;

namespace WatchMatchApi.ApiClients
{
    public class TMDbApi
    {
        private readonly IMemoryCache _cache;
        private readonly TMDbClient _client;
        private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromDays(1);

        public const int MAX_PAGES_LIMIT = 500;

        public TMDbApi(IMemoryCache cache, IConfiguration config)
            :this(cache, config, TimeSpan.FromDays(1))
        {}

        public TMDbApi(IMemoryCache cache, IConfiguration config, TimeSpan cacheExpirationTime)
        {
            _cache = cache;
            _cacheExpirationTime = cacheExpirationTime;
            _client = new TMDbClient(config["TMDB:API_KEY"]);
        }

        public DiscoverMovie CreateDiscoverQuery()
        {
            return _client.DiscoverMoviesAsync();
        }

        public async Task<SearchContainer<SearchMovie>> DiscoverMoviesAsync(int page)
        {
            string key = $"tmdb:discover:movies:{page}";
            return (await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheExpirationTime;
                return await _client.DiscoverMoviesAsync()
                                    .OrderBy(DiscoverMovieSortBy.PopularityDesc)
                                    .Query(page);
            }))!;
        }

        public async Task<Movie> GetMovieAsync(int movieId, MovieMethods includeMethods = 0)
        {
            string key = $"tmdb:movie:{movieId}:{includeMethods}";
            return (await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheExpirationTime;
                var movie = await _client.GetMovieAsync(movieId, includeMethods);
                return movie ?? throw new MovieNotFoundException(movieId);
            }))!;
        }
    }
}
