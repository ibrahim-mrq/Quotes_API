namespace Quotes.DTO.Responses
{
    public class QuoteResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }

    }
}
