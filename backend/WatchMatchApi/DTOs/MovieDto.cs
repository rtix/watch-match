using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TypeGen.Core.TypeAnnotations;

namespace WatchMatchApi.DTOs
{
    [ExportTsClass]
    public sealed class MovieDto
    {
        required public bool Adult { get; init; }
        required public string BackdropPath { get; init; }
        required public long Budget { get; init; }
        required public List<Genre> Genres { get; init; }
        required public int Id { get; init; }
        required public Images Images { get; init; }
        required public string ImdbId { get; init; }
        required public KeywordsContainer Keywords { get; init; }
        required public string OriginalLanguage { get; init; }
        required public string OriginalTitle { get; init; }
        required public string Overview { get; init; }
        public double? Popularity { get; init; }
        required public string PosterPath { get; init; }
        required public DateTime? ReleaseDate { get; init; }
        required public long Revenue { get; init; }
        public int? Runtime { get; init; }
        required public string Tagline { get; init; }
        required public string Title { get; init; }
        required public ResultContainer<Video> Videos { get; init; }
        required public double VoteAverage { get; init; }
        required public int VoteCount { get; init; }
    }
}
