namespace ChatChit.Models.ResponseModel
{
    public class UserResponseModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string? image {  get; set; }
        public string? nickName { get; set; }
        public bool? isFriend { get; set; }
    }
}
