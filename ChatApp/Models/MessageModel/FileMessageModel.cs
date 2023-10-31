using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models.MessageModel
{
    public class FileMessageModel
    {
        public string Content { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public DateTime? DateTime { get; set; }

        public bool? IsSeen { get; set; } = false;

        public bool? IsReply { get; set; } = false;

        public int? ReplyedToId { get; set; } = 0;

        public IFormFile File { get; set; }
        public string Type { get; set; }
    }
}
