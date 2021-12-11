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
        Task<Respose<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Respose<UserAppDto>> GeyUserByNameAsync(string userName);
    }
}
