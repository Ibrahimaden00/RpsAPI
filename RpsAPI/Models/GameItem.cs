using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RpsAPI.Models
{
    [Table("Game")]
    public class GameItem
    {

        [Key]
        public long GameID { get; set; }
       
        [ForeignKey("UserID")]
        public long InviteFromID { get; set; }
      
        [ForeignKey("UserID")]
        public long InviteToID { get; set; }
        public int? InviteFromIDMove { get; set; }
        public int? InviteToIDMove { get; set; }
        public string? Gamestatus { get; set; }


    }
}
