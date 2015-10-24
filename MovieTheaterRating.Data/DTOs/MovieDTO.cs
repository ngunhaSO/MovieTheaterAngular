using MovieTheaterRating.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterRating.Data.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Title { get; set; }
        
        [StringLength(4, ErrorMessage = "The {0} must have maximum of {1} characters long.", MinimumLength = 4)]
        [DisplayFormat(DataFormatString ="{20##}")]
        public string Year { get; set; }

        [Range(1, Int32.MaxValue)]
        public int Runtime { get; set; }

        public string MPAARating { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ReleaseDate { get; set; }

        public int AudienceRating { get; set; }

        [StringLength(2000, ErrorMessage = "The {0} must have maximum of {1} characters long.")]
        public string Synopsis { get; set; }

        public string PosterImg { get; set; }

        public string Thumbnail { get; set; }

        public ICollection<MovieCharacter> MovieCharacters { get; set; }
    }

    public class MovieCharacter
    {      
        public int ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharacterInMovie { get; set; }
    }
}
