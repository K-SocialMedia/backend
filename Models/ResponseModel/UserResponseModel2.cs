using static ChatChit.Models.FriendModel;

namespace ChatChit.Models.ResponseModel
{
    public class UserResponseModel2
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string? image { get; set; }
        public string? nickName { get; set; }
        public string? email { get; set; }
    }
}
