using System.IdentityModel.Tokens.Jwt;
using FilmApp.Business.Operations.User;
using FilmApp.Business.Operations.User.Dtos;
using FilmApp.WebApi.Jwt;
using FilmApp.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = FilmApp.WebApi.Models.LoginRequest;

namespace FilmApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Models.RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } // TODO: İleride action filter olarak kodlanacak

            var addUserDto = new AddUserDto()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                BirthDate = request.BirthDate,
            };

            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);

        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } // TODO: İleride action filter olarak kodlanacak

            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
                return BadRequest(result.Message);

            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            return Ok(new LoginResponse()
            {
                Message = "Giriş başarıyla tamamlandı.",
                Token = token
            });

        }

        [HttpGet("me")]
        [Authorize] // Token yoksa cevap yok!
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
