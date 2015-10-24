using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Entity
{
    public class Actor
    {
        public Actor()
        {
            ActorMovies = new List<ActorMovie>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<ActorMovie> ActorMovies { get; set; }
    }
}
