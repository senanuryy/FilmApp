using System.ComponentModel.DataAnnotations;

namespace FilmApp.WebApi.Models
{
    public class AddGenreRequest
    {
        [Required]
        [Length(2,40)]
        public string Title { get; set; }
    }
}
