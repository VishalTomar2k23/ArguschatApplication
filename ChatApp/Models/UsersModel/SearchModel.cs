using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models.UsersModel
{
    public class SearchModel
    {
        public string UserName { get; set; }

        public int  UserId { get; set; }

        public string firstName { get; set; }
        
        public DateTime dateTime { get; set; }

        public string content { get; set; }

    }
}
