namespace ChatApp.Models.GroupModel
{
    public class GroupMemberDetailModel
    {
        

        public int ProfileId { get; set; }
        public string? ImagePath { get; set; }
        public int Admin { get;  set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get;  set; }

        
    }
}
