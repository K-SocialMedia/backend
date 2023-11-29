using ChatChit.Models.Post;
using ChatChit.Models.ResponseModel;

namespace ChatChit.Models
{
    public class PostWithUserInfo
    {
        public Guid id { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string fullName { get; set; }
        public string? userImage { get; set; }
        public string? nickName { get; set; }
    }
}
