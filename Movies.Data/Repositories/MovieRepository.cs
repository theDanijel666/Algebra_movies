using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly moviesContext _context;

        public MovieRepository(moviesContext context)
        {
            _context = context;
        }

        public Movie Add(Movie movie)
        {
            var newMovie = _context.Movies.Add(movie);
            _context.SaveChanges();
            return newMovie.Entity;
        }

        public Movie Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return null;
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movies.Find(id);
        }

        public IEnumerable<Movie> QueryStringFilter(string s, string orderby, int per_page, int num_page)
        {
            var filter = _context.Movies.ToList();

            if (!String.IsNullOrEmpty(s))
            {
                filter = filter.Where(m => m.Title.Contains(s, StringComparison.CurrentCultureIgnoreCase)
                    || m.Genre.Contains(s, StringComparison.CurrentCultureIgnoreCase)
                    || m.ReleaseYear.Contains(s,StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            if (orderby.ToLower() == "asc")
            {
                filter=filter.OrderBy(m => m.Id).ToList();
            }
            if(orderby.ToLower() == "desc")
            {
                filter=filter.OrderByDescending(m => m.Id).ToList();
            }
            if(num_page<1) num_page = 1;
            if (per_page > 0 && num_page>0) {
                filter = filter.Skip((num_page - 1) * per_page).Take(per_page).ToList();
            }

            return filter;
        }

        public Movie Update(Movie movie)
        {
            var result = _context.Movies.Find(movie.Id);
            if (result != null)
            {
                result.Title = movie.Title;
                result.Genre = movie.Genre;
                result.ReleaseYear = movie.ReleaseYear;
                _context.SaveChanges();
                return result;
            }

            return null;
        }
    }
}
