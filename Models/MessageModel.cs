using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class MessageModel
    {
        [Column("id")]
        public int id { get; set; }
        [Column("sender_id")]
        public Guid senderId { get; set; }
        [Column("receiver_id")]
        public Guid receiverId { get; set; }
        [Column("content")]
        public string content { get; set; }
        [Column("create_at")]
        public DateTime createAt { get; set; }
        [Column("is_read")]
        public bool isRead { get; set; }
    }
}
