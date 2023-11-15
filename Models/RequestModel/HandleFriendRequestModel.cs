namespace ChatChit.Models.RequestModel
{
    public class HandleFriendRequestModel
    {
        public Guid friendId { get; set; }
        public FriendModel.FriendStatus status { get; set; }

    }
}
