using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data.Configurations
{
    public class ActorConfiguration : EntityTypeConfiguration<Actor>
    {
        public ActorConfiguration()
        {
            this.HasKey<int>(a => a.Id).Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.FirstName).IsRequired().HasMaxLength(20);
            this.Property(a => a.LastName).IsRequired().HasMaxLength(20);
            this.HasMany(a => a.ActorMovies);
        }
    }
}
