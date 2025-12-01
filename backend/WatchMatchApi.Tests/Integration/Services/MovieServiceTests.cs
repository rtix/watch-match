using Microsoft.Extensions.Configuration;
using WatchMatchApi.Services;

namespace WatchMatchApi.Tests.Integration.Services
{
    public class MovieServiceTests
    {
        private readonly IConfiguration _config;

        public MovieServiceTests()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            if (string.IsNullOrWhiteSpace(_config["TMDB:API_KEY"]))
                throw new Exception("TMDB API key missing — ensure it's in solution-level Manage User Secrets.");
        }

        [Fact]
        public async Task DiscoverRandomMovies_ReturnsMovieResults()
        {
            var service = new MovieService(_config);
            const int numberOfMovies = 5;

            var movies = await service.DiscoverRandomMovies(numberOfMovies);

            Assert.NotNull(movies);
            Assert.Equal(numberOfMovies, movies.Count);

            Assert.All(movies, m =>
            { 

                Assert.False(string.IsNullOrWhiteSpace(m.Title));
            });
        }
    }
}
