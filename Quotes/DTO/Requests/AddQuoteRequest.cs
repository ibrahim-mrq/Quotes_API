using Microsoft.AspNetCore.Mvc;

namespace Quotes.DTO.Requests
{
    public class AddQuoteRequest
    {

        public string? Name { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }

    }
}
