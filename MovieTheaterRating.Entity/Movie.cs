using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Entity
{
    public class Movie
    {
        public Movie()
        {
            ActorMovies = new List<ActorMovie>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Runtime { get; set; }

        public string Year { get; set; }

        public string MPAARating { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int AudienceRating { get; set; }

        public string Synopsis { get; set; }

        public string PosterImg { get; set; }

        public string Thumbnail { get; set; }

        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
    }
}
