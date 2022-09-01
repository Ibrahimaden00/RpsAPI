using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RpsAPI.Models
{

    [Table("Message")]
    public class MessageItem
    {
        [Key]
        public long MessageID { get; set; }
        [ForeignKey("UserID")]
        public long FromUserID { get; set; }
        [ForeignKey("UserID")]
        public long ToUserID { get; set; }
        public string ?MessageValue { get; set; }

    

    }
}
