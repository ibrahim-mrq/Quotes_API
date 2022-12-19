namespace Quotes.DTO.Responses
{
    public class FavoriteQuoteResponse
    {
        public string? Name { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

    }
}
