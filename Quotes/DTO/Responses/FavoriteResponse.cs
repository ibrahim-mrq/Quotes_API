using Quotes.Models;

namespace Quotes.DTO.Responses
{
    public class FavoriteResponse
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public int UserId { get; set; }
        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public bool IsFavorite { get; set; }
        public FavoriteQuoteResponse? Quote { get; set; }
    }
}
