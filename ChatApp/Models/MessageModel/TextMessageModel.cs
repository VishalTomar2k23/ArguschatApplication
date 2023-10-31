using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models.MessageModel
{
    public class TextMessageModel
    {
        
        public string Content { get; set; } = null!;

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public DateTime? DateTime { get; set; } //

        public int RepliedToId { get; set; }

        public string? RepliedContent { get; set; }

        public int IsReply { get; set; }

        public int IsSeen { get; set; }

        public string? Type { get; set; }
        public int? Id { get; set; }
    }
}
