using Microsoft.AspNetCore.Mvc;
using Movies.API.Controllers;
using Movies.Data.Models;
using Movies.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Test
{
    public class MoviesRepControllerTest
    {
        private readonly moviesContext _context;
        private readonly MoviesRepController _controller;
        private readonly MovieRepository _repository;

        public MoviesRepControllerTest()
        {
            _context = new moviesContext();
            _repository = new MovieRepository(_context);
            _controller = new MoviesRepController(_repository);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfCorrectCount()
        {
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result);

            var list=result.Result as OkObjectResult;

            Assert.IsType<List<Movie>>(list.Value);

            var movies = list.Value as List<Movie>;


            Assert.Equal(_context.Movies.Count(), movies.Count);
        }

        [Fact]
        public void GetAllMovies_ReturnSuccessIfWrongCount()
        {
            var result = _controller.GetMovies();

            Assert.IsType<OkObjectResult>(result.Result);

            var list = result.Result as OkObjectResult;

            Assert.IsType<List<Movie>>(list.Value);

            var movies = list.Value as List<Movie>;

            Assert.NotEqual(2, movies.Count);
        }

        [Theory]
        [InlineData(3, 666, "Full metal jacket")]
        [InlineData(5, 777, "GI Joe")]
        public void GetMovieById_ReturnsOkObjectResult(int id1, int id2, string title)
        {
            var okResult = _controller.GetMovie(id1);
            var notFoundResult= _controller.GetMovie(id2);

            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<NotFoundResult>(notFoundResult.Result);

            var item= okResult.Result as OkObjectResult;

            Assert.IsType<Movie>(item.Value);

            var movie= item.Value as Movie;
            Assert.Equal(title, movie.Title);
            Assert.Equal(id1, movie.Id);
        }


        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            var newMovie = new Movie()
            {
                Genre = "Crime-Drama",
                ReleaseYear = "1972"
            };

            _controller.ModelState.AddModelError("Title", "Required");

            var badResponse = _controller.PostMovie(newMovie);
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse_And_Remove_GetExistingMovieById_ReturnsOkObjectResult()
        {
            var newMovie = new Movie()
            {
                Id = 333,
                Title = "The Godfather",
                Genre = "Crime-Drama",
                ReleaseYear = "1972"
            };

            var createdResponse = _controller.PostMovie(newMovie);
            Assert.IsType<CreatedAtActionResult>(createdResponse);

            int id= ((createdResponse as CreatedAtActionResult).Value as Movie).Id;

            var okResult = _controller.DeleteMovie(id);
            Assert.IsType<OkObjectResult>(okResult);
        }


        [Theory]
        [InlineData(99999)]
        public void Remove_GetExistingMovieById_ReturnsNotFoundResult(int id)
        {
            var okResult = _controller.DeleteMovie(id);
            Assert.IsType<NotFoundObjectResult>(okResult);
        }

    }
}
