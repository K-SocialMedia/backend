namespace ChatChit.Models.ResponseModel
{
    public class UserResponse
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string? image {  get; set; }
        public string? nickName { get; set; }

    }
}
