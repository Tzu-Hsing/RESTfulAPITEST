namespace Myapi.DTOs
{
    public record AuthResponse
    {
        public string? AccessToken { get; init; }
        public string TokenType { get; init; } = "Bearer";
        public DateTime ExpiresIn { get; init; }

    }
}
 