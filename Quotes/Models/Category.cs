using System.ComponentModel.DataAnnotations;
using Quotes.Base;

namespace Quotes.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
