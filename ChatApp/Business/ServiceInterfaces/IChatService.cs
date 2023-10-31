using ChatApp.Context.EntityClasses;
using ChatApp.Models.MessageModel;
using ChatApp.Models.UsersModel;

namespace ChatApp.Business.ServiceInterfaces
{
    public interface IChatService
    {
        // we will declare all the function inside this file dealing with messages

        void SendMessage(TextMessageModel message);

        IEnumerable<TextMessageModel> GetMessages(string username, string selusername);

        int FetchUserIdByUsername(string username);

        void DeleteMessage(int MsgId);

        void ReplyMessage(TextMessageModel message, int MessageId); // message id for adding to is replyed to 

        bool CheckDeletedById(int Id);

        IEnumerable<SearchModel> SearchOthers(string searchname, string username); // we will check that search name is different from user name.

        public void MarkAsRead(string username, string selusername);

        public void SendFileMessage(FileMessageModel msg);

        public void ResponsesToUsersMessage(int sender, int reciever, TextMessageModel response);
    }        
}
