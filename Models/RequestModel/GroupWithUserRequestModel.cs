namespace ChatChit.Models.RequestModel
{
    public class GroupWithUserRequestModel
    {
        public string groupName {  get; set; }
        public Guid[] usersId {get; set; }
    }
}
