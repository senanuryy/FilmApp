using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmApp.Data.Entities
{
    public class GenreEntity : BaseEntity
    {
        public string Title { get; set; }

        // Relational Property
        public ICollection<MovieGenreEntity> MovieGenres { get; set; }
    }

    public class GenreConfiguration : BaseConfiguration<GenreEntity>
    {
        public override void Configure(EntityTypeBuilder<GenreEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
