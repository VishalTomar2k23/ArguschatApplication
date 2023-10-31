namespace ChatApp.Context.EntityClasses
{
    public class ConversationResult
    {

            public string FirstName { get; set; }

            public string UserName { get; set; }
            public string Content { get; set; }
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public DateTime DateTime { get; set; }
    

    }
}
