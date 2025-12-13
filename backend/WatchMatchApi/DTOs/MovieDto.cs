using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace WatchMatchApi.DTOs
{
    public sealed class MovieDto
    {
        public bool Adult { get; init; }
        public string? BackdropPath { get; init; }
        public long Budget { get; init; }
        public Credits? Credits { get; init; }
        public List<Genre>? Genres { get; init; }
        public int Id { get; init; }
        public Images? Images { get; init; }
        public string? ImdbId { get; init; }
        public KeywordsContainer? Keywords { get; init; }
        public string? OriginalLanguage { get; init; }
        public string? OriginalTitle { get; init; }
        public string? Overview { get; init; }
        public double? Popularity { get; init; }
        public string? PosterPath { get; init; }
        public DateTime? ReleaseDate { get; init; }
        public long Revenue { get; init; }
        public int? Runtime { get; init; }
        public string? Tagline { get; init; }
        public string? Title { get; init; }
        public ResultContainer<Video>? Videos { get; init; }
        public double VoteAverage { get; init; }
        public int VoteCount { get; init; }
    }
}
