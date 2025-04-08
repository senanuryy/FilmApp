using FilmApp.Business.Operations.Genre;
using FilmApp.Business.Operations.Genre.Dtos;
using FilmApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddGenre(AddGenreRequest request)
        {
            var addGenreDto = new AddGenreDto()
            {
                Title = request.Title,
            };

            var result = await _genreService.AddGenre(addGenreDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }

        
    }
}
