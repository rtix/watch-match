using TMDbLib.Objects.Movies;

namespace WatchMatchApi
{
    public class MovieLikesDTO
    {
        public required Movie Movie {  get; set; }
        public required int Likes { get; set; }
    }
}
