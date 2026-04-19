using Myapi.DTOs;
using Myapi.Services;

namespace Myapi.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse> RegisterRequestAsync(RegisterRequest request);
        Task<LoginResponse> LoginResponseAsync(LoginRequest request);

    }
}
