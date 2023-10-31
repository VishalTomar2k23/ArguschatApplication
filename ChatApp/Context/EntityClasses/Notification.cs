using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Context.EntityClasses;

public partial class Notification
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public int? GroupId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime DateTime { get; set; }

    
    public virtual Group? Group { get; set; }

    
    public virtual Profile Receiver { get; set; } 

    
    public virtual Profile Sender { get; set; } 
}
