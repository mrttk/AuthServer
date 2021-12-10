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
        Task<Responce<TokenDto>> CreateTokenAsync(LoginDto login);
        Task<Responce<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Responce<NoDataDto>> RevokeRefreshToken(string refreshToken);
        Task<Responce<ClientTokenDto>> CreateTokenByClient(ClientTokenDto clientTokenDto);
    }
}
