using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmApp.Data.Entities
{
    public class SessionEntity : BaseEntity
    {
        public int MovieId { get; set; }
        public string SessionNumber { get; set; }

        //  Relational Entity
        public ICollection<ReservationEntity> Reservations { get; set; }
        public MovieEntity Movie { get; set; }
    }
    public class SessionConfiguration : BaseConfiguration<SessionEntity>
    {
        public override void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            base.Configure(builder);
        }

    }
}
