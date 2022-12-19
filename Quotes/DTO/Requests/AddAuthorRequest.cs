using System.ComponentModel.DataAnnotations;

namespace Quotes.DTO.Requests
{
    public class AddAuthorRequest
    {
        public string? Name { get; set; }
        public int? BirthYear { get; set; }
        public string? BirthCountry { get; set; }
        public int? DeathYear { get; set; }

    }
}
