using System.ComponentModel.DataAnnotations;
using FilmApp.Data.Enums;
using Microsoft.AspNetCore.Antiforgery;

namespace FilmApp.WebApi.Models
{
    public class UpdateMovieRequest
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
