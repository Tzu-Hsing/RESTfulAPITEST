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
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _appdbcontext = context;
            _config = config;
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

            var token = GenerateJwtToken(user);

            user.LastLoginAt = DateTime.UtcNow;
            await _appdbcontext.SaveChangesAsync();

            return new LoginResponse { Success = true, Message="Login Successful", token= token };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username??string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId",user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
