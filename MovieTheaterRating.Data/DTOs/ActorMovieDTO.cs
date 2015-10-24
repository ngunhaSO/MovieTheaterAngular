using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data.DTOs
{
    public class ActorMovieDTO
    {
        public int ID { get; set; }

        public int ActorID { get; set; }

        public int MovieID { get; set; }

        public string CharacterInMovie { get; set; }

        public Movie Movie { get; set; }

        public Actor Actor { get; set; }
    }
}
