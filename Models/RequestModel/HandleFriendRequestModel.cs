namespace ChatChit.Models.RequestModel
{
    public class HandleFriendRequestModel
    {
        public string jwtToken {  get; set; }
        public Guid friendId { get; set; }
        public FriendModel.FriendStatus status { get; set; }

    }
}
