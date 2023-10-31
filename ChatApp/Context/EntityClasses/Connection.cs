using System;
using System.Collections.Generic;

namespace ChatApp.Context.EntityClasses;

public partial class Connection
{
    public int Id { get; set; }

    public int ProfileId { get; set; }

    public string? SignalId { get; set; }

    public DateTime DateTime { get; set; }

    public virtual Profile Profile { get; set; } = null!;
}
