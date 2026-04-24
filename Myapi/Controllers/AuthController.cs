using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Myapi.DAL;
using Myapi.DTOs;
using Myapi.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Myapi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iauthservice;
        public AuthController(IAuthService iauth)
        {
            _iauthservice = iauth;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerrequest)
        {
            if (registerrequest == null)
                return BadRequest("No request info");

            var result = await _iauthservice.RegisterRequestAsync(registerrequest);

            if (!result.Success)
                return BadRequest(result?.Message);


            return Ok(result);
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {

            if(request == null)
                return BadRequest("Login details are required.");

            var result = await _iauthservice.LoginResponseAsync(request);

            if (!result.Success)
                return Unauthorized(result?.Message);

            return Ok(result);


        }


    }
}
