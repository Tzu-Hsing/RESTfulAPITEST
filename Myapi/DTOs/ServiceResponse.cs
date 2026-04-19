namespace Myapi.DTOs
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

    }

    public class LoginResponse: ServiceResponse 
    {
        public string? token { get;set;  }
    
    }
}
