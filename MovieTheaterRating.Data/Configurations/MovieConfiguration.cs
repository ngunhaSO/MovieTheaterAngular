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
    public class MovieConfiguration : EntityTypeConfiguration<Movie>
    {
        public MovieConfiguration()
        {
            this.HasKey<int>(m => m.Id).Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(m => m.Title).IsRequired().HasMaxLength(50);
            this.Property(m => m.Year).IsOptional().HasMaxLength(4);
            this.Property(m => m.Runtime).IsOptional();
            this.Property(m => m.MPAARating).IsOptional();
            this.Property(m => m.ReleaseDate).IsOptional().HasColumnType("datetime2").HasPrecision(0);
            this.Property(m => m.AudienceRating).IsOptional();
            this.Property(m => m.Synopsis).IsOptional().HasMaxLength(2000);
            this.Property(m => m.PosterImg).IsOptional();
            this.Property(m => m.Thumbnail).IsOptional();
            this.HasMany(m => m.ActorMovies);
        }
    }
}
