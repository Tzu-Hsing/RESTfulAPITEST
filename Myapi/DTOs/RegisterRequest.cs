namespace Myapi.DTOs
{
    public record RegisterRequest
    {
        public string? Username { get; init; }
        public string? PasswordHash { get; init; } // Store the hashed password here
        public string? Email { get; init; }

        public string? FirstName { get; init; }
        public string? LastName { get; init; }

    }
}
