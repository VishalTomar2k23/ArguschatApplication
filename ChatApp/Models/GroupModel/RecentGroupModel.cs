namespace ChatApp.Models.GroupModel
{
    public class RecentGroupModel
    {
        public int Id { get; set; }

        public string GroupName { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }
        public string? LastMsg { get; internal set; }
        public string? LastMsgAt { get; internal set; }
        public int Type { get; internal set; }
    }
}
