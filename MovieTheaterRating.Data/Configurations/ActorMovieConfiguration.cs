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
    public class ActorMovieConfiguration : EntityTypeConfiguration<ActorMovie>
    {
        public ActorMovieConfiguration()
        {
            this.ToTable("ActorMovie");
            this.Property(am => am.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasKey(am => am.ID);
            this.HasRequired(am => am.Movie)
                .WithMany(m => m.ActorMovies)
                .HasForeignKey(am => am.MovieID)
                .WillCascadeOnDelete(false);
            this.HasRequired(am => am.Actor)
                .WithMany(a => a.ActorMovies)
                .HasForeignKey(am => am.ActorID)
                .WillCascadeOnDelete(false);
            this.Property(am => am.CharacterInMovie).HasMaxLength(100);
        }
    }
}
