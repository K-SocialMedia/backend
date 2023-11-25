using static ChatChit.Models.FriendModel;

namespace ChatChit.Models.ResponseModel
{
    public class UserResponseModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string? image {  get; set; }
        public string? nickName { get; set; }
        public FriendStatus  status { get; set; }
    }
}
