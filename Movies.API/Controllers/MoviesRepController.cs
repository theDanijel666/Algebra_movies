using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Data.Models;
using Movies.Data.Interfaces;
using System.Data.Odbc;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesRepController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesRepController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            try
            {
                return Ok(_movieRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            try
            {
                var movie=_movieRepository.GetById(id);
                if (movie == null) return NotFound();
                return Ok(movie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public ActionResult PostMovie([FromBody] Movie movie)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newMovie=_movieRepository.Add(movie);

                return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new movie.");
            }
        }

        [HttpPut("{id}")]
        public ActionResult PutMovie(int id,Movie movie)
        {
            try
            {
                if(id!=movie.Id) return BadRequest("Movie ID mismatch");

                var movieToUpdate=_movieRepository.GetById(id);

                if (movieToUpdate == null) return NotFound("Movie to update not found.");

                return Ok(_movieRepository.Update(movie));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating movie.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                var movieToDelete = _movieRepository.GetById(id);
                if (movieToDelete == null) return NotFound("Movie to delete not found.");

                return Ok(_movieRepository.Delete(id));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting.");
            }
        }

        [HttpGet("filter")]
        public ActionResult SearchByQueryString([FromQuery] string s="",
                                                [FromQuery] string orderby="asc",
                                                [FromQuery] int per_page=0,
                                                [FromQuery] int page=0)
        {
            try
            {
                var movies = _movieRepository.QueryStringFilter(s, orderby, per_page, page);

                return Ok(movies);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrievieng data");
            }
        }

    }
}
