using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Business.Operations.Movie.Dto;
using FilmApp.Business.Types;
using FilmApp.Data.Entities;
using FilmApp.Data.Repositories;
using FilmApp.Data.UnitOfWork;

namespace FilmApp.Business.Operations.Movie
{
    public class MovieManager : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MovieEntity> _movieRepository;
        private readonly IRepository<MovieGenreEntity> _movieGenreRepository;

        public MovieManager(IUnitOfWork unitOfWork, IRepository<MovieEntity> movieRepository, IRepository<MovieGenreEntity> movieGenreRepository)
        {
            _unitOfWork = unitOfWork;
            _movieRepository = movieRepository;
            _movieGenreRepository = movieGenreRepository;
        }

        public async Task<ServiceMessage> AddMovie(AddMovieDto movie)
        {
            var hasMovie = _movieRepository.GetAll(x => x.Name.ToLower() == movie.Name.ToLower()).Any();

            if (hasMovie)
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Bu film zaten sistemde mevcut."
                };
            }

            await _unitOfWork.BeginTransaction();

            var movieEntity = new MovieEntity()
            {
                Name = movie.Name,
                IMDB = movie.IMDB,
                Language = movie.Language,
                FormatType = movie.FormatType,
            };

            _movieRepository.Add(movieEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Film kaydı sırasında bir sorunla karşılaşıldı.");
            }

            foreach (var genreId in movie.GenreIds)
            {
                var movieGenre = new MovieGenreEntity()
                {
                    MovieId = movieEntity.Id,
                    GenreId = genreId,
                };

                _movieGenreRepository.Add(movieGenre);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();
                throw new Exception("Film özellikleri eklenirken bir hatayla katşılaşıldı, süreç başa sarıldı.");
            }

            return new ServiceMessage()
            {
                IsSucceed = true,
            };
        }
    }
}
