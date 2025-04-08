using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Business.Operations.Genre.Dtos;
using FilmApp.Business.Types;

namespace FilmApp.Business.Operations.Genre
{
    public interface IGenreService
    {
        Task<ServiceMessage> AddGenre(AddGenreDto genre);
    }
}
