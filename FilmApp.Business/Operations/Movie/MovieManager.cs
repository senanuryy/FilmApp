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
using Microsoft.EntityFrameworkCore;

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

        public async Task<ServiceMessage> AdjustMovieImdb(int id, int changeTo)
        {
            var movie = _movieRepository.GetById(id);

            if (movie is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eşleşen film bulunamadı."
                };
            }
            movie.IMDB = changeTo;

            _movieRepository.Update(movie);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("IMDB puanı değiştirilirken bir hata oluştu.");
            }

            return new ServiceMessage()
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> DeleteMovie(int id)
        {
            var movie = _movieRepository.GetById(id);

            if (movie == null)
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Silinmek istenen film bulunamadı."
                };
            }

            _movieRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw new Exception("Silme işlemi sırasında bir hata oluştu.");
            }

            return new ServiceMessage()
            {
                IsSucceed = true
            };
        }

        public async Task<MovieDto> GetMovie(int id)
        {
            var movie = await _movieRepository.GetAll(x => x.Id == id)
                .Select(x => new MovieDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IMDB = x.IMDB,
                    Language = x.Language,
                    FormatType = x.FormatType,
                    Genres = x.MovieGenres.Select(x => new MovieGenreDto
                    {
                        Id = x.Id,
                        Title = x.Genre.Title
                    }).ToList()
                }).FirstOrDefaultAsync();

            return movie;
        }

        public async Task<List<MovieDto>> GetMovies()
        {
            var movies = await _movieRepository.GetAll()
               .Select(x => new MovieDto()
               {
                   Id = x.Id,
                   Name = x.Name,
                   IMDB = x.IMDB,
                   Language = x.Language,
                   FormatType = x.FormatType,
                   Genres = x.MovieGenres.Select(x => new MovieGenreDto
                   {
                       Id = x.Id,
                       Title = x.Genre.Title
                   }).ToList()
               }).ToListAsync();

            return movies;
        }

        public async Task<ServiceMessage> UpdateMovie(UpdateMovieDto movie)
        {
            var movieEntity = _movieRepository.GetById(movie.Id);

            if (movieEntity is null)
            {
                return new ServiceMessage()
                {
                    IsSucceed = false,
                    Message = "Film bulunamadı."
                };
            }

            await _unitOfWork.BeginTransaction();

            movieEntity.Name = movie.Name;
            movieEntity.IMDB = movie.IMDB;
            movieEntity.Language = movie.Language;
            movieEntity.FormatType = movie.FormatType;

            _movieRepository.Update(movieEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransaction();

                throw new Exception("Film bilgileri güncellebirken bir hata ile karşılaşıldı.");
            }

            var movieGenres = _movieGenreRepository.GetAll(x => x.MovieId == x.MovieId).ToList();

            foreach (var movieGenre in movieGenres)
            {
                _movieGenreRepository.Delete(movieGenre, false); // HARD DELETE
            }

            foreach (var genreId in movie.GenreIds)
            {
                var movieGenre = new MovieGenreEntity()
                {
                    MovieId = movieEntity.Id,
                    GenreId = genreId
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
                throw new Exception("Film bilgileri güncellenirken bir hata oluştu. İşlemleri geriye alınıyor.");
            }

            return new ServiceMessage()
            {
                IsSucceed = true,
            };
        }
    }
}
