using ChatApp.Models.GroupModel;

namespace ChatApp.Business.ServiceInterfaces
{
    public interface IGroupService
    {
        public RecentGroupModel CreateGroup(string userName, CreateGroupModel grp);
        public IEnumerable<GroupModel> GetRecentGroups(string userName);
        public IEnumerable<GroupMemberDetailModel> getAllMembers(int groupId);
        public GroupModel getGroup(int groupId, string username);

        // To get the people which can be added to group and not currently members
        public IEnumerable<GroupMemberDetailModel> getAllProfiles(int groupId, string userName);
        public IEnumerable<GroupMemberDetailModel> addMembersToGroup(int grpId, string[] selUsers, string userName);
        public GroupModel updateGroup(string userName, CreateGroupModel grp, int grpId);
        public void leaveGroup(string userName, int groupId);
        public void makeAdmin(int groupId, string selUserName, string userName);
        public void removeUser(int groupId, string selUserName, string username);
        public void SendFileMessage(GroupInputMessageModel msg);
        public IEnumerable<GroupOutputMessageModel> GetAllMessage(int groupId);
    }

}
