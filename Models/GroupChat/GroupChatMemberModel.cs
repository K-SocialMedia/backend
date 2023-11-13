using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.GroupChat
{
    public class GroupChatMemberModel
    {
        [Key]
        [Column("id")]
        public Guid id;
    }
}
