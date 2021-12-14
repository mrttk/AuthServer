using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Responce<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Responce<UserAppDto>> GeyUserByNameAsync(string userName);
    }
}
