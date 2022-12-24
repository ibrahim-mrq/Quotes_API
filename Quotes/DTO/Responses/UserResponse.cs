using System.ComponentModel.DataAnnotations;

namespace Quotes.DTO.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
      
    }
}
