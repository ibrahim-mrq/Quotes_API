namespace Quotes.DTO.Requests
{
    public class DeleteFavoriteRequest
    {
        public int? UserId { get; set; }
        public int? FavoriteId { get; set; }

    }
}
