﻿using AuthServer.Core.DTOs;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Respose<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Respose<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            return Respose<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Respose<UserAppDto>> GeyUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user ==null)
            {
                return Respose<UserAppDto>.Fail("UserName not found!", 404, true);
            }

            return Respose<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

        }
    }
}
