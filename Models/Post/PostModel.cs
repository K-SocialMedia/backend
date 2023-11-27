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
        [Column ("content")]
        public string? content { get; set; }
        [Column ("image")]
        public string image { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
        [Column("updated_at")]
        public DateTime updatedAt { get; set; }
    }
}
