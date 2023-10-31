using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context.EntityClasses;
using ChatApp.Context;
using ChatApp.Models.MessageModel;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Models.GroupModel;

namespace ChatApp
{
    public class Chathub : Hub
    {
        private readonly ArgusChatContext context;
        private readonly IChatService chatService;

        public Chathub(ArgusChatContext context, IChatService chatservice)
        {
            this.context = context;
            this.chatService = chatservice;
        }


        // build in method for starting the connection when we start connection
        #region OneToOneHub
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task ConnectDone(string userName)
        {
            string curSignalId = Context.ConnectionId;
            Profile user = context.Profiles.FirstOrDefault(p => p.UserName == userName);

            if (user != null)
            {
                if (this.context.Connections.Any(u => u.ProfileId == user.Id))
                {
                    IEnumerable<Connection> users = this.context.Connections.Where(u => u.ProfileId == user.Id);
                    this.context.Connections.RemoveRange(users);
                    context.SaveChanges();
                }

                Connection connUser = new Connection()
                {
                    ProfileId = user.Id,
                    SignalId = curSignalId,
                    DateTime = DateTime.Now,
                };
                await context.Connections.AddAsync(connUser);
                context.SaveChanges();

                await Clients.Client(curSignalId).SendAsync("ResponseSuccess");
            }
            else
            {
                await Clients.Client(curSignalId).SendAsync("ResponseFail");
            }
            //await Clients.Caller.SendAsync("ResponseSuccess", user);
        }

        public void ConnectRemove(string userName)
        {
            if (userName != null)
            {
                int userId = context.Profiles.FirstOrDefault(u => u.UserName == userName).Id;
                var connect = context.Connections.FirstOrDefault(u => u.ProfileId == userId);
                context.Connections.Remove(connect);
                context.SaveChanges();
            }
            else
            {
                return;
            }
        }

        public async Task sendMsg(TextMessageModel message)
        {
            Message newMessage = null;
            TextMessageModel response = null;
            string replyMessage;
            int messageFromId = message.SenderId;
            int messageToId = message.ReceiverId;
            newMessage = new Message
            {
                Content = message.Content,
                DateTime = DateTime.Now,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                RepliedToId = message.RepliedToId,
                IsReply = message.IsReply,
                RepliedContent = message.RepliedContent,
                IsSeen = 0,
                Type = "Null",
            };
            context.Messages.Add(newMessage);
            context.SaveChanges();
            if (message.RepliedToId == 0)
            {
                replyMessage = null;
            }
            else
            {
                var msg = context.Messages.FirstOrDefault(msg => msg.Id == message.RepliedToId);
                replyMessage = msg.Content;
            }
            response = new TextMessageModel
            {
                Id = newMessage.Id,
                RepliedContent = newMessage.RepliedContent,
                Content = newMessage.Content,
                DateTime = DateTime.Now,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                RepliedToId = message.RepliedToId,
                IsReply = message.RepliedToId,
                IsSeen = 0,
                Type = "Null",
            };
            this.chatService.ResponsesToUsersMessage(message.SenderId, message.ReceiverId, response);
        }


        #endregion

        #region GroupHub

        public async Task sendGroupMsg(GroupInputMessageModel message)
        {
            GroupMessage newMessage = null;
            GroupOutputMessageModel response = null;
            string replyMessage;
            int messageFromId = message.SenderId;
            int groupId = message.GroupId;
            newMessage = new GroupMessage
            {
                Content = message.Content,
                CreatedAt = DateTime.Now,
                SenderId = messageFromId,
                GroupId = groupId,
                RepliedToId = (int)message.RepliedToId,
                Type = null,
            };
            context.GroupMessages.Add(newMessage);
            context.SaveChanges();
            if (message.RepliedToId == 0)
            {
                replyMessage = "";
            }
            else
            {
                var msg = context.GroupMessages.FirstOrDefault(msg => msg.Id == message.RepliedToId);
                replyMessage = msg.Content;
            }
            var profile = context.Profiles.FirstOrDefault(p => p.Id == messageFromId);
            response = new GroupOutputMessageModel
            {
                Id = newMessage.Id,
                Content = newMessage.Content,
                CreatedAt = (DateTime)newMessage.CreatedAt,
                SenderId = message.SenderId,
                RepliedContent = replyMessage,
                RepliedToId = (int)newMessage.RepliedToId,
                IsReply = message.IsReply,
                Type = null,
            };
            var groupMemberIds = context.GroupMembers.Where(u => u.GroupId == groupId).Select(u => u.ProfileId).ToList();
            foreach (var memberId in groupMemberIds)
            {
                var connection = context.Connections.FirstOrDefault(u => u.ProfileId == memberId);
                if (connection != null)
                {
                    await Clients.Clients(connection.SignalId).SendAsync("RecieveMessageGroup", response);
                }
            }
        }
        #endregion

    }

}
