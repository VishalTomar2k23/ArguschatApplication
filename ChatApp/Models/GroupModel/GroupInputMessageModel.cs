namespace ChatApp.Models.GroupModel
{
    public class GroupInputMessageModel
    {
        // We can use this model for taking file messages and text messages.
        public int GroupId { get; set; }

        public string Content { get; set; } = null!;

        public int SenderId { get; set; }

        

        public DateTime CreatedAt { get; set; }

        public int RepliedToId { get; set; }

        public string? RepliedContent { get; set; }

        public int IsReply { get; set; }

        public string? Type { get; set; }

        public IFormFile? File { get; set; }
    }
}
