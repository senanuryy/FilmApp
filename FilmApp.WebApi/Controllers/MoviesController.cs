using FilmApp.Business.Operations.Movie;
using FilmApp.Business.Operations.Movie.Dto;
using FilmApp.WebApi.Filters;
using FilmApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovie(id);

            if (movie == null)
                return NotFound();
            else
                return Ok(movie);
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetMovies();

            return Ok(movies);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMovie(AddMovieRequest request)
        {
            var addMovieDto = new AddMovieDto()
            {
                Name = request.Name,
                IMDB = request.IMDB,
                Language = request.Language,
                FormatType = request.FormatType,
                GenreIds = request.GenreIds,
            };

            var result = await _movieService.AddMovie(addMovieDto);

            if (!result.IsSucceed)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPatch("{id}/imdb")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustMovieImdb(int id, int changeTo)
        {
            var result = await _movieService.AdjustMovieImdb(id, changeTo);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovie(id);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [TimeControlFilter]
        public async Task<IActionResult> UpdateMovie(int id, UpdateMovieRequest request)
        {
            var updateMovielDto = new UpdateMovieDto()
            {
                Id = id,
                Name = request.Name,
                IMDB = request.IMDB,
                Language = request.Language,
                FormatType = request.FormatType,
                GenreIds = request.GenreIds,
            };

            var result = await _movieService.UpdateMovie(updateMovielDto);

            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return await GetMovie(id);

        }
    }
}
