using System;
using System.Collections.Generic;

namespace ChatApp.Context.EntityClasses;

public partial class Group
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public string? ImagePath { get; set; }

    public string? Description { get; set; }

    public virtual Profile? CreatedByNavigation { get; set; }

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

    public virtual ICollection<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
