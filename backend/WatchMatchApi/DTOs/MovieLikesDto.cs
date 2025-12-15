using TypeGen.Core.TypeAnnotations;

namespace WatchMatchApi.DTOs
{
    [ExportTsInterface]
    public sealed class MovieLikesDto
    {
        public required MovieDto Movie { get; init; }
        public required int Likes { get; init; }
    }
}
