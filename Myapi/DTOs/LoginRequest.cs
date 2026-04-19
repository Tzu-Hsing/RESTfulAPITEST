namespace Myapi.DTOs
{
    public record LoginRequest
    {
        public string? Username { get; init; }
        public string? Password { get; init; }

    }
}
