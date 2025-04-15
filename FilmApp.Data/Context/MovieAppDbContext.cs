using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static FilmApp.Data.Entities.MovieEntity;
using static FilmApp.Data.Entities.MovieGenreEntity;

namespace FilmApp.Data.Context
{
    public class MovieAppDbContext : DbContext
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Fluent Api

            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new MovieGenreConfiguration());            
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());            
            modelBuilder.ApplyConfiguration(new SessionConfiguration());            
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            modelBuilder.Entity<SettingEntity>().HasData(
                new SettingEntity()
                {
                    Id = 1,
                    MaintenenceMode = false,
                });

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<GenreEntity> Genres => Set<GenreEntity>();
        public DbSet<MovieEntity> Movies => Set<MovieEntity>();
        public DbSet<MovieGenreEntity> MovieGenres => Set<MovieGenreEntity>();
        public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();
        public DbSet<SessionEntity> Sessions => Set<SessionEntity>();
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();
    }
}
