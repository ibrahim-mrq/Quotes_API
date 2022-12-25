namespace Quotes.DTO.Requests.Other
{
    public class AddAuthorRequest
    {
        public string? Name { get; set; }
        public int? BirthYear { get; set; }
        public string? BirthCountry { get; set; }
        public int? DeathYear { get; set; }

    }
}
