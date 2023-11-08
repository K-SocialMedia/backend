using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class FriendModel
    {
        [Column("user_id")]
        public string userId { get; set; }
        [Column("friend_id")]
        public string friendId { get; set; }

        public UserModel User { get; set; }
        public UserModel UserFriend {  get; set; }
    }
}
