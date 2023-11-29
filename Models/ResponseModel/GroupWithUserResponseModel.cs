namespace ChatChit.Models.ResponseModel
{
    public class GroupWithUserResponseModel
    {
        public Guid id { get; set; }
        public string groupName { get; set; }
        public List<Guid> userId { get; set; }
    }
}
