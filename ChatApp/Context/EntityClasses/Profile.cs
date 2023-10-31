using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Context.EntityClasses;

public partial class Profile
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int ProfileType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? LastUpdatedAt { get; set; }

    public int? LastUpdatedBy { get; set; }

    public string? ImagePath { get; set; }

    public int Designation { get; set; }

    public string? Status { get; set; }

    public int? IsDeleted { get; set; }

    public virtual Connection? Connection { get; set; }

    public virtual ICollection<GroupMember> GroupMembers { get; set; } = new List<GroupMember>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    [NotMapped]
    public virtual ICollection<Notification> NotificationReceivers { get; set; } = new List<Notification>();

    [NotMapped]
    public virtual ICollection<Notification> NotificationSenders { get; set; } = new List<Notification>();
}
