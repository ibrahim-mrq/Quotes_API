using Quotes.Base;
using System.ComponentModel.DataAnnotations;

namespace Quotes.Models
{
    public class Token : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public string? Value { get; set; }


        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Boolean IsActive { get; set; }

    }
}
