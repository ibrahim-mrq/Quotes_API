namespace Quotes.DTO.Requests.User
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public string? Password { get; set; }
    }
}
