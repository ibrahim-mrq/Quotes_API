using System.ComponentModel.DataAnnotations;
using Quotes.Base;

namespace Quotes.Models
{
    public class User : BaseEntity
    {

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceToken { get; set; }

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpirationToken { get; set; }
        public string? Code { get; set; }
        public DateTime? ExpirationCode{ get; set; }

    }
}

