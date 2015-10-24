using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must have maximum of {1} characters long", MinimumLength = 1)]
        public string FirstName { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must have maximum of {1} characters long", MinimumLength =  1)]
        public string LastName { get; set; }

        public ICollection<ActorMovie> ActorMovies { get; set; }
    }
}
