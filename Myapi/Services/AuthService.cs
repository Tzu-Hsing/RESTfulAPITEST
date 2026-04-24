using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Myapi.DAL;
using Myapi.DTOs;
using Myapi.Interfaces;
using Myapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Myapi.Services
{
    public class AuthService: IAuthService
    {
        private readonly AppDbContext _appdbcontext;
        private readonly ITokenService _tokenService;
        public AuthService(AppDbContext context, ITokenService tokenService)
        {
            _appdbcontext = context;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse> RegisterRequestAsync(RegisterRequest request) 
        {
            //check user exist
            if (await _appdbcontext.Users.AnyAsync(x => x.Username == request.Username))
                return new ServiceResponse{ Success = false, Message="UserName already registered." }; ;

            //check email
            if (await _appdbcontext.Users.AnyAsync(x => x.Email == request.Email))
                return new ServiceResponse { Success = false, Message="Email is already taken."};

            try
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                    CreatedAt = DateTime.UtcNow,
                    isActive = true,
                    
                };

                _appdbcontext.Users.Add(user);
                await _appdbcontext.SaveChangesAsync();

                return new ServiceResponse { Success = true, Message = "Registered Successful!" };

            }
            catch(Exception ex)
            {
                return new ServiceResponse { Success = false, Message = $"A database error occurred. errors: {ex?.Message}" };
            }

        }


        public async Task<LoginResponse> LoginResponseAsync(LoginRequest loginrequest)
        {

            var user = await _appdbcontext.Users.FirstOrDefaultAsync(x => x.Username == loginrequest.Username);

            if (user == null)
                return new LoginResponse { Success = false, Message = "User not found" };

            if (!BCrypt.Net.BCrypt.Verify(loginrequest.Password, user.PasswordHash))
                return new LoginResponse { Success = false, Message = "Wrong Password" };

            var token = _tokenService.CreateToken(user);

            user.LastLoginAt = DateTime.UtcNow;
            await _appdbcontext.SaveChangesAsync();

            return new LoginResponse { Success = true, Message="Login Successful", token= token };
        }


    }
}
