using System.ComponentModel.DataAnnotations;
using Quotes.Base;
using Quotes.DTO.Responses;

namespace Quotes.Models
{
    public class Favorite : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public int UserId { get; set; }

    }
}
