using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.Post
{
    public class PostImagesModel
    {
        [Key]
        [Column ("id")]
        public Guid id { get; set; }
        [Column("post_id")]
        public Guid postId { get; set; }
        [Column("url")]
        public string url { get; set; }
        [Column("created_at")]
        public string createdAt { get; set; }
        [Column("updated_at")]
        public string updatedAt { get; set; }

    }
}
