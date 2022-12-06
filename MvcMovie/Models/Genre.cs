using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string GenreName { get; set; }


    }
}
