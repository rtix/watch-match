namespace WatchMatchApi.Exceptions
{
    public class MovieNotFoundException(int movieId) : Exception($"Movie {movieId} not found")
    {
    }
}
