using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class FriendModel
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("user_id")]
        public Guid userId { get; set; }
        [Column("friend_id")]
        public Guid friendId { get; set; }
        [Column("status")]
        public FriendStatus? status {  get; set; }

        [ForeignKey("userId")]
        public UserModel? User { get; set; }

        [ForeignKey("friendId")]
        public UserModel? Friend { get; set; }
       
        public enum FriendStatus
        {
            Rejected = -1,
            Pending = 0,
            Accepted = 1
        }
    }
}