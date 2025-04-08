using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmApp.Data.Entities
{
    public class MovieGenreEntity : BaseEntity
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }

        // Relational Property

        public MovieEntity Movie { get; set; }
        public GenreEntity Genre { get; set; }

        public class MovieGenreConfiguration : BaseConfiguration<MovieGenreEntity>
        {
            public override void Configure(EntityTypeBuilder<MovieGenreEntity> builder)
            {
                builder.Ignore(x => x.Id);  // Id propertysini görmezden geldik, tabloya aktarılmayacak.
                builder.HasKey("MovieId", "GenreId"); // Composite Key oluşturup yeni Primary Key olarak atadık.
                base.Configure(builder);
            }
        }
    }
}
