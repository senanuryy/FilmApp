using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FilmApp.WebApi.Models
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public String  Token { get; set; }
    }
}
