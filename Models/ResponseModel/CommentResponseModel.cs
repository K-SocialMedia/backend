namespace ChatChit.Models.ResponseModel
{
    public class CommentResponseModel
    {
        public string content { get; set; }
        public string ownerName { get; set; }
        public string ownerImage { get; set; }
        public DateTime createdAt { get; set; }
    }
}
