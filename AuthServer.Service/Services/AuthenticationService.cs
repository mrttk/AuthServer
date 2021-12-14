using AuthServer.Core.Condiguration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;
        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenService = userRefreshTokenService;
        }
        public async Task<Respose<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Respose<TokenDto>.Fail("Email or password is wrong!", 400, true);

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Respose<TokenDto>.Fail("Email or password is wrong!", 400, true);

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });


            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Respose<TokenDto>.Success(token, 200);

        }

        public Respose<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
            {
                return Respose<ClientTokenDto>.Fail("ClientId or ClientSecret not found!", 404, true);
            }

            var token = _tokenService.CreateTokenByClient(client);

            return Respose<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Respose<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
            {
                return Respose<TokenDto>.Fail("Refresh token not found!", 404, true);
            }

            var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (user == null)
            {
                return Respose<TokenDto>.Fail("UserId not found!", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Respose<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<Respose<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var exitRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (exitRefreshToken == null)
            {
                return Respose<NoDataDto>.Fail("Refresh token not found!", 404, true);
            }

            _userRefreshTokenService.Remove(exitRefreshToken);

            await _unitOfWork.CommitAsync();

            return Respose<NoDataDto>.Success(200);
        }
    }
}
