namespace ChatChit.Models.ResponseModel
{
    public class MessageGroupResponseModel
    {
        public Guid id { get; set; }
        public Guid senderId { get; set; }
        public string senderName { get; set; }
        public string senderImage {  get; set; }
        public string content { get; set; }
        public string? image { get; set; }
        public DateTime createAt { get; set; }
        public bool isRead { get; set; }
    }
}
