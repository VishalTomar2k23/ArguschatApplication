using System;
using System.Collections.Generic;

namespace ChatApp.Context.EntityClasses;

public partial class GroupMember
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public int GroupId { get; set; }

    public DateTime JoinedAt { get; set; }

    public int Admin { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
