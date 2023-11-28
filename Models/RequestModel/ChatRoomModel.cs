namespace ChatChit.Models.RequestModel
{
    public class ChatRoomModel
    {
        public string? tokenUserId { get; set; }
        public Guid? friendId { get; set; }
        public string? roomId { get; set; }
    }
}
