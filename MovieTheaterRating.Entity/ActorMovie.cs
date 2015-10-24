using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Entity
{
    public class ActorMovie
    {
        public int ID { get; set; }

        public int ActorID { get; set; }

        public int MovieID { get; set; }

        public string CharacterInMovie { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual Actor Actor { get; set; }
    }
}
