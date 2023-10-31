using System;
using System.Collections.Generic;

namespace ChatApp.Context.EntityClasses;

public partial class GroupMessage
{
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Content { get; set; } = null!;

    public int SenderId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int RepliedToId { get; set; }

    public string? RepliedContent { get; set; }

    public int IsReply { get; set; }

    public string? Type { get; set; }

    public virtual Group Group { get; set; } = null!;
}
