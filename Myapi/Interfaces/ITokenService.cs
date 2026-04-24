using Myapi.Models;

namespace Myapi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
