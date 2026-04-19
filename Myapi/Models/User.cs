using Microsoft.AspNetCore.Identity;
using Myapi.Settings;

namespace Myapi.Models
{
    // This represents your SQL Table
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; } // Store the hashed password here
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? isActive { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public string Role { get; set; } = CommonCode.Role.Member;
        

    }
}
