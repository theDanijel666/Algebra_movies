using Movies.Data.Models;

namespace Movies.Data.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAll();
        Movie GetById(int id);
        Movie Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(int id);

        IEnumerable<Movie> QueryStringFilter(string s, string orderby, int per_page, int num_page);
    }
}
