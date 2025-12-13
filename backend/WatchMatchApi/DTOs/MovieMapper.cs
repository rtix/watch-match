using TMDbLib.Objects.Movies;

namespace WatchMatchApi.DTOs
{
    public class MovieMapper
    {
        public static MovieDto ToDto(Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                OriginalLanguage = movie.OriginalLanguage,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Adult = movie.Adult,
                ReleaseDate = movie.ReleaseDate,
                Runtime = movie.Runtime,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                VoteAverage = movie.VoteAverage,
                VoteCount = movie.VoteCount,
                Popularity = movie.Popularity,
                PosterPath = movie.PosterPath,
                BackdropPath = movie.BackdropPath,
                Genres = movie.Genres,
                Images = movie.Images,
                ImdbId = movie.ImdbId,
                Keywords = movie.Keywords,
                Videos = movie.Videos
            };
        }
    }
}
