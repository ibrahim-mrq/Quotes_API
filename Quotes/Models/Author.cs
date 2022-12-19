using Quotes.Base;
using System.ComponentModel.DataAnnotations;

namespace Quotes.Models
{
    public class Author : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int BirthYear { get; set; }
        public string? BirthCountry { get; set; }
        public int DeathYear { get; set; }

    }
}
