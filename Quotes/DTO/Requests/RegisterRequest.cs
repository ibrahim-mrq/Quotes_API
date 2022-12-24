namespace Quotes.DTO.Requests
{
    public class RegisterRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceToken { get; set; }

    }
}
