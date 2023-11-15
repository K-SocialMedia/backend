using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models.Post
{
    public class PostModel
    {
        [Key]
        [Column ("id")]
        public Guid id { get; set; }
        [Column ("ownerId")]
        public Guid ownerId { get; set; }
        public string? content { get; set; }
        [Column("created_at")]
        public DateTime createAt { get; set; }
        [Column("update_at")]
        public DateTime updateAt { get; set; }

    }
}
