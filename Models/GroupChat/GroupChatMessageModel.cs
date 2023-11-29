using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.GroupChat
{
    public class GroupChatMessageModel
    {
        [Key]
        [Column ("id")]
        public Guid id { get; set; }
        [Column("senderId")]
        public Guid senderId { get; set; }
        [Column("image")]
        public string? image { get; set; }
        [Column ("group_id")]
        public Guid groupId { get; set; }
        [Column("content")]
        public string content { get; set; }
        [Column("create_at")]
        public DateTime createAt { get; set; }
        [Column("is_read")]
        public bool isRead { get; set; }
    }
}
