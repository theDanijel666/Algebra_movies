using System.ComponentModel.DataAnnotations;

namespace Movies.Data.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ReleaseYear { get; set; }
    }
}
