using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context.EntityClasses;
using ChatApp.Context;
using ChatApp.Models.GroupModel;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Group = ChatApp.Context.EntityClasses.Group;

namespace ChatApp.Infrastructure.ServiceImplementation
{
    public class GroupServices : IGroupService
    {
        private readonly ArgusChatContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IChatService chatService;
        private readonly IHubContext<Chathub> hubContext;

        public GroupServices
            (ArgusChatContext context, IWebHostEnvironment webHostEnvironment,
            IChatService chatService, IHubContext<Chathub> hubContext)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
            this.chatService = chatService;
            this.hubContext = hubContext;
        }

        // We will take input as People who are requested to join the group
        public IEnumerable<GroupMemberDetailModel> addMembersToGroup(int groupId, string[] selUsers, string userName)
        {
            var allProfiles = new List<GroupMemberDetailModel>();
            int userId = chatService.FetchUserIdByUsername(userName);
            List<int> selUserIds = new List<int>();
            if (context.GroupMembers.FirstOrDefault(u => u.ProfileId == userId).Admin == 1)
            {
                foreach (var selUser in selUsers)
                {
                    int selUserId = chatService.FetchUserIdByUsername(selUser);
                    selUserIds.Add(selUserId);
                    if (!context.GroupMembers.Any(u => u.GroupId == groupId && u.ProfileId == selUserId))
                    {
                        GroupMember newMember = new GroupMember
                        {
                            GroupId = groupId,
                            ProfileId = selUserId,
                            JoinedAt = DateTime.Now,
                            Admin = 0,
                        };
                        context.GroupMembers.Add(newMember);
                        context.SaveChanges();

                        Profile profile = context.Profiles.FirstOrDefault(u => u.UserName == selUser);
                        //Response
                        var newObj = new GroupMemberDetailModel
                        {
                            ImagePath = profile.ImagePath,
                            Admin = newMember.Admin,
                            FirstName = profile.FirstName,
                            LastName = profile.LastName,
                            UserName = profile.UserName,
                        };
                        allProfiles.Add(newObj);
                    }
                }

                var sortedMessages = context.GroupMessages.OrderByDescending(m => m.CreatedAt);
                Group group = context.Groups.FirstOrDefault(m => m.Id == groupId);

                // We will use a stored procedure to return Group details
                var newGrp = new RecentGroupModel();
                
                foreach (var selId in selUserIds)
                {
                    var connect = context.Connections.FirstOrDefault(u => u.ProfileId == selId);
                    if (connect != null)
                    {
                        this.hubContext.Clients.Client(connect.SignalId).SendAsync("iAmAddedToGroup", newGrp);
                    }
                }
                return allProfiles;
            }
            return null;
        }

        public RecentGroupModel CreateGroup(string userName, CreateGroupModel grp)
        {
            Group newGroup = new Group
            {
                GroupName = grp.GroupName,
                CreatedAt = DateTime.Now,
                CreatedBy = chatService.FetchUserIdByUsername(userName),
            };
            if (grp.Description != null)
            {
                newGroup.Description = grp.Description;
            }
            if (grp.File != null)
            {
                var filename = Guid.NewGuid().ToString(); // new generated image file name
                var uploads = Path.Combine(webHostEnvironment.WebRootPath, @"grpimages");
                var extension = Path.GetExtension(grp.File.FileName);// Get Extension Of the File

                using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    grp.File.CopyTo(fileStreams);
                }

                newGroup.ImagePath = "/grpimages/" + filename + extension;
            }
            context.Groups.Add(newGroup);
            context.SaveChanges();
            GroupMember GroupMember = new GroupMember
            {
                GroupId = newGroup.Id,
                ProfileId = (int)newGroup.CreatedBy,
                JoinedAt = DateTime.Now,
                Admin = 1
            };

            context.GroupMembers.Add(GroupMember);
            context.SaveChanges();
            RecentGroupModel response = new RecentGroupModel
            {
                Id = newGroup.Id,
                GroupName = newGroup.GroupName,
                Type = 0,
                LastMsg = null,
                LastMsgAt = null,
            };
            if (grp.File != null)
            {
                response.ImagePath = newGroup.ImagePath;
            }
            return response;
        }

        public IEnumerable<GroupMemberDetailModel> getAllMembers(int groupId)
        {
            var groupMemberLists = new List<GroupMemberDetailModel>();
            var members = context.GroupMembers.Where(m => m.GroupId == groupId);
            foreach (var member in members)
            {
                var memberProfile = context.Profiles.FirstOrDefault(m => m.Id == member.ProfileId);
                var newObj = new GroupMemberDetailModel
                {
                    ImagePath = memberProfile.ImagePath,
                    Admin = member.Admin,
                    FirstName = memberProfile.FirstName,
                    LastName = memberProfile.LastName,
                    UserName = memberProfile.UserName,
                };
                groupMemberLists.Add(newObj);
            }
            return groupMemberLists;

        }

        public IEnumerable<GroupOutputMessageModel> GetAllMessage(int groupId)
        {
            
            var returnList = new List<GroupOutputMessageModel>();
            var response = new List<GroupOutputMessageModel>();
            var groupMessages = context.GroupMessages.Where(u => u.GroupId == groupId).ToList();
            if (groupMessages.Count > 0)
            {
                foreach (var groupMessage in groupMessages)
                {
                    var profile = context.Profiles.FirstOrDefault(u => u.Id == groupMessage.SenderId);
                    var newObj = new GroupOutputMessageModel
                    {
                        Id = groupMessage.Id,
                        Content = groupMessage.Content,
                        SenderId = profile!.Id,
                        CreatedAt = (DateTime)groupMessage.CreatedAt,
                        Type = groupMessage.Type,
                    };
                    if (groupMessage.RepliedToId == 0)
                    {
                        newObj.RepliedContent = null;
                    }
                    else
                    {
                        var message = context.GroupMessages.FirstOrDefault(u => u.Id == groupMessage.RepliedToId);
                        newObj.RepliedContent = message.Content;
                    }
                    returnList.Add(newObj);
                }
                response = returnList.OrderBy(e => e.CreatedAt).ToList();
                return response;
            }
            return null;
        }

        // To Search the members which can be added to group and making sure it doesnot include existing members.
        public IEnumerable<GroupMemberDetailModel> getAllProfiles(int groupId, string userName)
        {
            var allProfiles = new List<GroupMemberDetailModel>();
            List<int> groupMemberId = new List<int>();
            groupMemberId = context.GroupMembers.Where(u => u.GroupId == groupId).Select(u => u.ProfileId).ToList();
            int userId = chatService.FetchUserIdByUsername(userName);
            var profiles = context.Profiles.Where(u => !groupMemberId.Contains(u.Id));
            if (context.GroupMembers.FirstOrDefault(u => u.GroupId == groupId && u.ProfileId == userId).Admin == 1)
            {
                foreach (var profile in profiles)
                {
                    var profileModel = new GroupMemberDetailModel
                    {
                        UserName = profile.UserName,
                        ImagePath = profile.ImagePath,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                    };
                    allProfiles.Add(profileModel);
                }
                return allProfiles;
            }
            return null;
        }

        public void makeAdmin(int groupId, string selUserName, string userName)
        {
            int userId = chatService.FetchUserIdByUsername(userName);
            int selUserId = chatService.FetchUserIdByUsername(selUserName);
            if (context.GroupMembers.Any(u => u.GroupId == groupId && u.ProfileId == userId && u.Admin == 1))
            {
                GroupMember member = context.GroupMembers.FirstOrDefault(u => u.GroupId == groupId && u.ProfileId == selUserId && u.Admin == 0);
                if (member != null)
                {
                    member.Admin = 1;
                    context.GroupMembers.Update(member);
                    context.SaveChanges();

                    // We can use this method to make the user admin without needing user to relaunch and we can also inform other about the change instantenously 
                    var connect = context.Connections.FirstOrDefault(u => u.ProfileId == selUserId);
                    if (connect != null)
                    {
                        this.hubContext.Clients.Client(connect.SignalId).SendAsync("MadeMeAdmin", groupId);
                    }
                }
            }
        }

        public void removeUser(int groupId, string selUserName, string username)
        {
            int userId = chatService.FetchUserIdByUsername(username);
            int selUserId = chatService.FetchUserIdByUsername(selUserName);
            if (context.GroupMembers.Any(u => u.GroupId == groupId && u.ProfileId == userId && u.Admin == 1))
            {
                GroupMember member = context.GroupMembers.FirstOrDefault(u => u.GroupId == groupId && u.ProfileId == selUserId && u.Admin == 0);
                if (member != null)
                {
                    
                    context.GroupMembers.Remove(member);
                    context.SaveChanges();

                    // We can use this method to remove the user and we can also inform other about the change instantenously 
                    var connect = context.Connections.FirstOrDefault(u => u.ProfileId == selUserId);
                    if (connect != null)
                    {
                        this.hubContext.Clients.Client(connect.SignalId).SendAsync("iAmRemovedFromGroup", groupId);
                    }
                }
            }
        }

        // Method to add 
        public void SendFileMessage(GroupInputMessageModel msg)
        {
            // message file is optional but since it is file Message we will ensure it contains file 
            if(msg.File == null)
            {
                return;
            }
            var message = new GroupMessage();
            var filename = Guid.NewGuid().ToString(); // new generated image file name
            var extension = Path.GetExtension(msg.File.FileName);// Get Extension Of the File
            var filetype = msg.File.ContentType.Split('/')[0];
            string uploads;
            if (filetype == "audio")
            {
                uploads = Path.Combine(webHostEnvironment.WebRootPath, @"group/audio");
                message.Content = "/group/audio/" + filename + extension;
                message.Type = "audio";
            }
            else if (filetype == "video")
            {
                uploads = Path.Combine(webHostEnvironment.WebRootPath, @"group/videos");
                message.Content = "/group/videos/" + filename + extension;
                message.Type = "video";
            }
            else
            {
                uploads = Path.Combine(webHostEnvironment.WebRootPath, @"group/images");
                message.Content = "/group/images/" + filename + extension;
                message.Type = "image";
            }

            using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
            {
                msg.File.CopyTo(fileStreams);
            }
         
            message.GroupId = msg.GroupId;
            message.SenderId = msg.SenderId;
            message.RepliedToId = 0; // we are not allowing user to reply file messages as reply to a message.
            message.CreatedAt = DateTime.Now;

            context.GroupMessages.Add(message);
            context.SaveChanges();

            var response = new GroupOutputMessageModel()
            {
                Id = message.Id,
                Content = message.Content,
                CreatedAt = (DateTime)message.CreatedAt,
                SenderId = msg.SenderId,
                RepliedToId = 0,
                Type = message.Type,
            };
            var groupMemberIds = context.GroupMembers.Where(u => u.GroupId == msg.GroupId).Select(u => u.ProfileId).ToList();
            var connections = context.Connections.Where(u => groupMemberIds.Contains(u.ProfileId)).Select(u => u.SignalId).ToList();
            this.hubContext.Clients.Clients(connections).SendAsync("RecieveMessageGroup", response);
        }

        public GroupModel updateGroup(string userName, CreateGroupModel group, int groupId)
        {
            int userId = chatService.FetchUserIdByUsername(userName);
            if (context.GroupMembers.FirstOrDefault(u => u.GroupId == groupId && u.ProfileId == userId).Admin == 1)
            {
                Group existingGroup = context.Groups.FirstOrDefault(u => u.Id == groupId);
                existingGroup.GroupName = group.GroupName;
                existingGroup.Description = group.Description;
                if (group.File != null)
                {
                    var filename = Guid.NewGuid().ToString(); // new generated image file name
                    var uploads = Path.Combine(webHostEnvironment.WebRootPath, @"grpimages");
                    var extension = Path.GetExtension(group.File.FileName);// Get Extension Of the File

                    if (existingGroup.ImagePath != null)
                    {
                        var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath + existingGroup.ImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        group.File.CopyTo(fileStreams);
                    }
                    existingGroup.ImagePath = "/grpimages/" + filename + extension;
                }
                context.Groups.Update(existingGroup);
                context.SaveChanges();
                Profile profile = context.Profiles.FirstOrDefault(u => u.Id == userId);
                var groupModel = new CreateGroupModel
                {
                    Id = groupId,
                    ImagePath = existingGroup.ImagePath,
                    CreatedAt = existingGroup.CreatedAt,
                    GroupName = existingGroup.GroupName,
                    Description = existingGroup.Description,
                    CreatedBy = existingGroup.CreatedBy,
                };
                //SignalR
                List<int> groupMembersId = context.GroupMembers.Where(u => u.GroupId == groupId).Select(u => u.ProfileId).ToList();
                var connect = context.Connections.Where(u => groupMembersId.Contains(u.ProfileId)).Select(u => u.SignalId);
                this.hubContext.Clients.Clients(connect).SendAsync("GroupUpdated", groupModel);
            }
            return null;
        }

        public GroupModel getGroup(int groupId, string username)
        {
            var group = context.Groups.FirstOrDefault(u => u.Id == groupId);
            if (group != null)
            {
                int userId = chatService.FetchUserIdByUsername(username);
                if (context.GroupMembers.Any(u => u.GroupId == groupId && u.ProfileId == userId))
                {
                    var groupModel = new GroupModel
                    {
                        Id = group.Id,
                        ImagePath = group.ImagePath,
                        CreatedAt = group.CreatedAt,
                        GroupName = group.GroupName,
                        Description = group.Description,
                        CreatedBy = group.CreatedBy,
                    };
                    return groupModel;
                }
            }
            return null;
        }

        // we will creating a stored procedure similar to recentUsers in Messages.
        public IEnumerable<GroupModel> GetRecentGroups(string userName)
        {
            throw new NotImplementedException();
        }


        // When the user is himself exiting the group. 
        public void leaveGroup(string userName, int groupId)
        {
            // We Will have to check some edge cases 
            // if User leaves the group then will there be any admin or not . If No admin then we will make the first user to join admin. 
            // if User leaves will the group be empty if yes then remove group and delete all related information from the database.
     
            int admins = context.GroupMembers.Where(u => u.GroupId == groupId && u.Admin == 1).Count();
            int userId = chatService.FetchUserIdByUsername(userName);
            int numberOfUsers = context.GroupMembers.Where(u => u.GroupId == groupId).Count();
            if (numberOfUsers == 1)
            {
                IEnumerable<GroupMessage> groupmessages = context.GroupMessages.Where(u => u.GroupId == groupId).ToList();
                if (groupmessages != null)
                {
                    context.GroupMessages.RemoveRange(groupmessages);
                    context.SaveChanges();
                }
            }
            GroupMember member = context.GroupMembers.FirstOrDefault(u => u.GroupId == groupId && u.ProfileId == userId);

            if (member != null)
            {
                context.RemoveRange(member);
                context.SaveChanges();
                if (admins == 1 && numberOfUsers > 1 && member.Admin == 1)
                {
                    GroupMember firstMember = context.GroupMembers.Where(u => u.GroupId == groupId).OrderBy(u => u.JoinedAt).First();
                    firstMember.Admin = 1;
                    context.GroupMembers.UpdateRange(firstMember);
                    context.SaveChanges();
                    var connect = context.Connections.FirstOrDefault(u => u.ProfileId == firstMember.ProfileId);
                    if (connect != null)
                    {
                        this.hubContext.Clients.Client(connect.SignalId).SendAsync("MadeMeAdmin", groupId);
                    }
                }
                if (numberOfUsers == 1)
                {
                    Group group = context.Groups.FirstOrDefault(u => u.Id == groupId);
                    context.Groups.RemoveRange(group);
                    context.SaveChanges();
                }
            }
        }
    }
}
