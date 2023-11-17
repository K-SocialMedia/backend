using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatChit.Models
{
    public class TokenModel
    {
        [Key]
        [Column("id")]
        public Guid id { get; set; }
        [Column("user_id")]
        public Guid userId { get; set; }
        [Column("token")]
        public string token { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
    }
}
