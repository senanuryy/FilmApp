using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Business.Operations.Genre.Dtos;
using FilmApp.Business.Types;
using FilmApp.Data.Entities;
using FilmApp.Data.Repositories;
using FilmApp.Data.UnitOfWork;

namespace FilmApp.Business.Operations.Genre
{
    public class GenreManager : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<GenreEntity> _repository;

        public GenreManager(IUnitOfWork unitOfWork, IRepository<GenreEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ServiceMessage> AddGenre(AddGenreDto genre)
        {
            var hasGenre = _repository.GetAll(x => x.Title.ToLower() == genre.Title.ToLower()).Any();

            if (hasGenre)
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Genre zaten bulunuyor."
                };
            }

            var genreEntity = new GenreEntity()
            {
                Title = genre.Title,
            };

            _repository.Add(genreEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Genre kaydı sırasında bir hata oluştu.");
            }

            return new ServiceMessage()
            {
                IsSucceed = true
            };
        }
    }
}
