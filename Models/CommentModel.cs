using ChatChit.Models.Post;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class CommentModel
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("owner_id")]
        public Guid ownerId {  get; set; }
        [Column("post_id")]
        public Guid postId { get; set; }
        [Column("content")]
        public string content { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
        [Column("updated_at")]
        public DateTime updatedAt { get; set; }

        [ForeignKey("ownerId")]
        public UserModel User { get; set; }
        [ForeignKey("postId")]
        public PostModel Post { get; set; }
    }
}
