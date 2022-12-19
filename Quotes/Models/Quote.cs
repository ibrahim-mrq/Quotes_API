using System.ComponentModel.DataAnnotations;
using Quotes.Base;

namespace Quotes.Models
{
    public class Quote : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

    }
}
