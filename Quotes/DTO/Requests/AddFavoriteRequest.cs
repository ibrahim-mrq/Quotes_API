namespace Quotes.DTO.Requests
{
    public class AddFavoriteRequest
    {
        public int? UserId { get; set; }
        public int? QuoteId { get; set; }

    }
}
