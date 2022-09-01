using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RpsAPI.Models
{
    [Table("User")]
    public class UserItem
    {

        [Key]
        public long UserID { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenIssued { get; set; }
        public string ? ResetToken { get; set; }    

    }
}
