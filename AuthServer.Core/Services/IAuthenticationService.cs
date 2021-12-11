using AuthServer.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Respose<TokenDto>> CreateTokenAsync(LoginDto login);
        Task<Respose<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Respose<NoDataDto>> RevokeRefreshToken(string refreshToken);
        Task<Respose<ClientTokenDto>> CreateTokenByClient(ClientTokenDto clientTokenDto);
    }
}
