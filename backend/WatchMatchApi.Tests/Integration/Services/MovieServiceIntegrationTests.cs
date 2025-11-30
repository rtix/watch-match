namespace WatchMatchApi.Tests.Integration.Services
{
    using Microsoft.Extensions.Configuration;
    using WatchMatchApi.Services;
    using Xunit;

    public class MovieServiceIntegrationTests
    {
        private readonly IConfiguration _config;

        public MovieServiceIntegrationTests()
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

            var movies = await service.DiscoverRandomMovies();

            Assert.NotEmpty(movies);
            Assert.All(movies, m => Assert.False(string.IsNullOrWhiteSpace(m.Title)));
        }
    }
}
