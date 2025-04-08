using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FilmApp.Data.Entities
{
    public class ReservationEntity : BaseEntity
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GuestCount { get; set; }

        //  Relational Property
        public UserEntity User { get; set; }
        public SessionEntity Session { get; set; }
    }
    public class ReservationConfiguration : BaseConfiguration<ReservationEntity>
    {
        public override void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
