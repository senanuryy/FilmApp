using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmApp.Business.Operations.User.Dtos;
using FilmApp.Business.Types;

namespace FilmApp.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user); // async çünkü unit of work kullanılacak

        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
