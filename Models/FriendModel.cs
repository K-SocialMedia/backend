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