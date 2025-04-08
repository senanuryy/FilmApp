using System.ComponentModel.DataAnnotations;
using FilmApp.Data.Enums;

namespace FilmApp.WebApi.Models
{
    public class AddMovieRequest
    {
        [Required]
        public string Name { get; set; }
        public int? IMDB { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public FormatType FormatType { get; set; }

        public List<int> GenreIds { get; set; }
    }
}
