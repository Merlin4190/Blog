using AutoMapper;
using DecaBlog.Commons.Helpers;
using DecaBlog.Helpers;
using DecaBlog.Models;
using DecaBlog.Models.DTO;
using DecaBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DecaBlog.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userMgr;
        private readonly SignInManager<User> _signInMgr;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<User> userManager, IMapper mapper, IJwtService jwtService, SignInManager<User> signInManager)
        {
            _userMgr = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _signInMgr = signInManager;
        }

        public async Task<string> ForgotPassword(string email)
        {
            var user = await _userMgr.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var isConfirmed = await _userMgr.IsEmailConfirmedAsync(user);
            if (!isConfirmed)
            {
                return null;
            }
            var token = await _userMgr.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<LoginResponseDto> Login(LoginDto model)
        {
            var user = await _userMgr.FindByEmailAsync(model.Email);
            var result = await _signInMgr.PasswordSignInAsync(user, model.Password, model.Rememberme, false);
            if (!result.Succeeded)
                return null;
            var userRoles = await _userMgr.GetRolesAsync(user);
            var token = _jwtService.GenerateJWTToken(user, userRoles);
            return new LoginResponseDto
            {
                FullName = $"{user.FirstName} {user.LastName}",
                UserId = user.Id,
                Role = userRoles,
                Token = token
            };
        }
    }
}