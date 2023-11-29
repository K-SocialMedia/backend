using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.GroupChat
{
    public class GroupChatMemberModel
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }

        [Column("user_id")]
        public Guid userId { get; set; }

        [Column("group_id")]
        public Guid groupId { get; set; }
    }
}
