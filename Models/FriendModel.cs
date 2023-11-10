using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class FriendModel
    {
        [Key]
        public Guid id { get; set; }
        public Guid userId { get; set; }
        public Guid friendId { get; set; }

        [ForeignKey("userId")]
        public UserModel User { get; set; }

        [ForeignKey("friendId")]
        public UserModel Friend { get; set; }
    }
}