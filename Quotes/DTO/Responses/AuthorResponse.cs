using System.ComponentModel.DataAnnotations;

namespace Quotes.DTO.Responses
{
    public class AuthorResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int BirthYear { get; set; }
        public string? BirthCountry { get; set; }
        public int DeathYear { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
