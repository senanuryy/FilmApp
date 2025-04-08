using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmApp.Data.Entities
{
    public class MovieEntity : BaseEntity
    {
        public string Name { get; set; }
        public int? IMDB { get; set; }
        public string Language { get; set; }
        public FormatType FormatType { get; set; }

        // Relational Property

        public ICollection<MovieGenreEntity> MovieGenres { get; set; }

        public class MovieConfiguration : BaseConfiguration<MovieEntity>
        {
            public override void Configure(EntityTypeBuilder<MovieEntity> builder)
            {
                builder.Property(x => x.IMDB)
                    .IsRequired(false);

                builder.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                base.Configure(builder);
            }
        }
    }
}
