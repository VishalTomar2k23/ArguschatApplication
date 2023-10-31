namespace ChatApp.Models.GroupModel
{
    public class GroupMemberModel
    {
        public int Id { get; set; }

        public int ProfileId { get; set; }

        public int GroupId { get; set; }

        public DateTime JoinedAt { get; set; }

        public int Admin { get; set; }
    }
}
