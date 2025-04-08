using FilmApp.Business.Operations.Movie;
using FilmApp.Business.Operations.Movie.Dto;
using FilmApp.WebApi.Models;
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

        [HttpPost]
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
    }
}
