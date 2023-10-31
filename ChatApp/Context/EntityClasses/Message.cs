using System;
using System.Collections.Generic;

namespace ChatApp.Context.EntityClasses;

public partial class Message
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public DateTime DateTime { get; set; }

    public int RepliedToId { get; set; }

    public string? RepliedContent { get; set; }

    public int IsReply { get; set; }

    public int IsSeen { get; set; }

    public string? Type { get; set; }
}
