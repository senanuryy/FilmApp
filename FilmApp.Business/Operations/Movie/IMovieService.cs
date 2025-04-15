using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Business.Operations.Movie.Dto;
using FilmApp.Business.Types;

namespace FilmApp.Business.Operations.Movie
{
    public interface IMovieService
    {
        Task<ServiceMessage> AddMovie(AddMovieDto movie);
        Task<MovieDto> GetMovie(int id);
        Task<List<MovieDto>> GetMovies();
        Task<ServiceMessage> AdjustMovieImdb(int id, int changeTo);
        Task<ServiceMessage> DeleteMovie(int id);

        Task<ServiceMessage> UpdateMovie(UpdateMovieDto movie);
    }
}
