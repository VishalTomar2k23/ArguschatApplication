using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context;
using ChatApp.Context.EntityClasses;
using ChatApp.Models.MessageModel;
using ChatApp.Models.UsersModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

namespace ChatApp.Infrastructure.ServiceImplementation
{
    public class ChatServices : IChatService
    {
        private readonly ArgusChatContext context;
        private readonly IWebHostEnvironment environment;
        private readonly IHubContext<Chathub> hubContext;

        public ChatServices(ArgusChatContext context, IWebHostEnvironment environment, IHubContext<Chathub> hubContext
            )
        {
            this.context = context;
            this.environment = environment;
            this.hubContext = hubContext;
        }
        // we will just uploading the data files to database 
        public void SendMessage(TextMessageModel message)
        {
            // first check if the reciever is valid or not 
            var profile = context.Profiles.FirstOrDefault(profile => profile.Id == message.ReceiverId);
            if (profile != null && message.Content != null) // checking if the profile is valid and message is not null 
            {

                Message msg = new Message
                {
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    IsReply = message.RepliedToId == 0 ? 0 : 1,
                    RepliedToId = message.RepliedToId,
                    Content = message.Content,
                    IsSeen = 0,
                    DateTime = DateTime.Now,
                    Type = "Null"
                };
                context.Messages.Add(msg);
                context.SaveChanges();
            }
        }

        // for implementing this we need to get id from username
        public IEnumerable<TextMessageModel> GetMessages(string username, string selusername)
        {
            int userId = FetchUserIdByUsername(username);
            int seluserId = FetchUserIdByUsername(selusername);

            // Get all messages between both users 
            var list = context.Messages.Where(masg => (masg.SenderId == userId && masg.ReceiverId == seluserId) || (masg.SenderId == seluserId && masg.ReceiverId == userId));
            // Create a empty list of messages to return back 
            var returnList = new List<TextMessageModel>();
            var response = new List<TextMessageModel>();
            // for each message in list we will add that message to l
            foreach (var content in list)
            {

                var obj = new TextMessageModel();
                // will update if needed 

                obj.SenderId = userId == content.SenderId ? userId : seluserId;
                obj.ReceiverId = seluserId == content.ReceiverId ? seluserId : userId;
                obj.Content = content.Content;
                obj.DateTime = content.DateTime;
                obj.Type = content.Type;
                // Now implement the IsSeen for both senerios 
                if (content.SenderId == userId)
                {
                    obj.IsSeen = content.IsSeen ;
                }
                else
                {
                    obj.IsSeen = 1;
                    content.IsSeen = 1;
                }

                returnList.Add(obj);
            }
            context.SaveChanges();

            response = returnList.OrderBy(t => t.DateTime).ToList();

            return response;
        }



        public int FetchUserIdByUsername(string username)
        {
            Profile user = context.Profiles.FirstOrDefault(profile => profile.UserName == username);
            return user.Id;
        }

        public void DeleteMessage(int MsgId)
        {
            var msg = context.Messages.FirstOrDefault(m => m.Id == MsgId);
            if (msg.Content != null || msg != null) // message not already deleted.
            {
                msg.Content = "";

                context.SaveChanges();
            }
        }

        public bool CheckDeletedById(int Id)
        {
            var msg = context.Messages.FirstOrDefault(m => m.Id == Id);
            return msg.Content == "";
        }
        public void ReplyMessage(TextMessageModel message, int MessageId)
        {
            // If we are trying to reply to a deleted message
            if (CheckDeletedById(MessageId))
            {
                return;
            }
            var profile = context.Profiles.FirstOrDefault(profile => profile.Id == message.ReceiverId);
            if (profile != null && message.Content != null) // checking if the profile is valid and message is not null 
            {
                Message msg = new Message();
                msg.SenderId = message.SenderId;
                msg.ReceiverId = message.ReceiverId;
                msg.IsReply = 1;
                msg.RepliedToId = MessageId;
                msg.Content = message.Content;
                msg.IsSeen = message.IsSeen;
                msg.DateTime = DateTime.Now;
                context.Messages.Add(msg);
                context.SaveChanges();
            }
        }

        public IEnumerable<SearchModel> SearchOthers(string searchname, string username)
        {
            var profiles = context.Profiles.Where(u => (u.FirstName.StartsWith(searchname) || u.LastName.StartsWith(searchname) || u.UserName.StartsWith(searchname)) && u.UserName != username);
            profiles = profiles.OrderBy(u => u.FirstName);
            var list = new List<SearchModel>();

            foreach (var profile in profiles)
            {
                var usrId = FetchUserIdByUsername(username);
                var otherId = profile.Id;
                var conversations = context.ConversationResults.FromSqlRaw("EXEC dbo.GetAllConversationByUserIdsBoth @UserID, @OtherID", new SqlParameter("UserID", usrId), new SqlParameter("OtherID", otherId)).ToList();
                var tocontent = "";
                var toDateTime = DateTime.Now;
                if (conversations.Any())
                {
                    tocontent = conversations.ElementAt(0).Content;
                    toDateTime = conversations.ElementAt(0).DateTime;
                }
 
                SearchModel Temp = new SearchModel()
                {
                    UserName = profile.UserName,
                    UserId = profile.Id,
                    firstName = profile.FirstName,
                    content = tocontent,
                    dateTime = toDateTime

                };
                list.Add(Temp);
            }
            return list;
        }


        public void MarkAsRead(string username, string selusername)
        {
            // we will start with getting the message where we are reciever then we will if we are reciever then we will mark the messages as seen. 

            List<Message> msgs = null;
            var CurUserId = FetchUserIdByUsername(username);
            if (selusername == "All")
            {
                msgs = context.Messages.Where(m => m.ReceiverId == CurUserId).ToList();


            }
            else
            {
                var SelUserId = FetchUserIdByUsername(selusername);
                msgs = context.Messages.Where(m => m.SenderId == SelUserId && m.ReceiverId == CurUserId).ToList();

            }
            foreach (var msg in msgs)
            {
                msg.IsSeen = 1;
            }
            context.SaveChanges();


        }

        // will have to create new parameters into messages type and path 
        public void SendFileMessage(FileMessageModel msg)
        {
            // we will have almost same logic as profile image load 

            // checking if the user exists or not. we can use content part to store the file address as it will be empty in the start.

            var profile = context.Profiles.Where(u => u.Id == msg.ReceiverId);
            if (profile != null)
            {
                var message = new Message();
                var filename = Guid.NewGuid().ToString(); // new generated image file name
                var extension = Path.GetExtension(msg.File.FileName);// Get Extension Of the File
                var filetype = msg.File.ContentType.Split('/')[0];
                string uploads;
                if (filetype == "audio")
                {
                    uploads = Path.Combine(environment.WebRootPath, @"chat/audio");
                    message.Content = "/chat/audio/" + filename + extension;
                    message.Type = "audio";
                }
                else if (filetype == "video")
                {
                    uploads = Path.Combine(environment.WebRootPath, @"chat/videos");
                    message.Content = "/chat/videos/" + filename + extension;
                    message.Type = "video";
                }
                else
                {
                    uploads = Path.Combine(environment.WebRootPath, @"chat/images");
                    message.Content = "/chat/images/" + filename + extension;
                    message.Type = "image";
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    msg.File.CopyTo(fileStreams);
                }

                message.SenderId = msg.SenderId ;
                message.ReceiverId = msg.ReceiverId;
                message.IsReply = msg.ReplyedToId == 0 ? 0 : 1;
                message.RepliedToId = (int)msg.ReplyedToId;
                
                message.IsSeen = 0;
                message.DateTime = DateTime.Now;
                
                context.Messages.Add(message);
                context.SaveChanges();

                var response = new TextMessageModel()
                {
                    Id = message.Id,
                    Content = message.Content,
                    DateTime = message.DateTime,
                    SenderId = msg.SenderId,
                    ReceiverId = msg.ReceiverId,
                    RepliedToId = 0,
                    IsSeen = 0,
                    Type = message.Type,
                };
                ResponsesToUsersMessage(message.SenderId, message.ReceiverId, response);

            }
        }

        public void ResponsesToUsersMessage(int sender, int reciever, TextMessageModel response)
        {
            Connection Sender = this.context.Connections.FirstOrDefault(u => u.ProfileId == reciever
            );
            Connection Receiver = this.context.Connections.FirstOrDefault(u => u.ProfileId == sender
            );
            if (Receiver != null)
            {
                this.hubContext
                    .Clients.Clients(Sender.SignalId, Receiver.SignalId).SendAsync("recieveMessage", response);
            }
            else
            {
                this.hubContext.Clients.Client(Sender.SignalId).SendAsync("recieveMessage", response);
            }
        }
    }
}
