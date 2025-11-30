using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace WatchMatchApi.Services
{
    public class MovieService(IConfiguration config)
    {
        private readonly TMDbClient _client = new(config["TMDB:API_KEY"]);
        private const int MAX_PAGES_LIMIT = 500;

        private async Task<SearchContainer<SearchMovie>> GetPage(int number)
        {
            return await _client.DiscoverMoviesAsync().OrderBy(TMDbLib.Objects.Discover.DiscoverMovieSortBy.PopularityDesc).Query(number);
        }

        public async Task<List<SearchMovie>> DiscoverRandomMovies(int moviesCount=10)
        {
            SearchMovie[] randomMovies = new SearchMovie[moviesCount];
            var maxPages = Math.Min(GetPage(1).Result.TotalPages, MAX_PAGES_LIMIT);
            var random = new Random();
            await Task.Run(() =>
            {
                Parallel.For(0, moviesCount, i =>
                {
                    var randomPageNumber = random.Next(1, maxPages + 1);
                    var randomPage = GetPage(randomPageNumber);
                    var numberOfItemsInPage = randomPage.Result.Results.Count;
                    var randomItemNumber = random.Next(0, numberOfItemsInPage);
                    var randomItem = randomPage.Result.Results[randomItemNumber];
                    randomMovies[i] = randomItem;
                });
            });
            return randomMovies.ToList(); 
        }
    }
}
