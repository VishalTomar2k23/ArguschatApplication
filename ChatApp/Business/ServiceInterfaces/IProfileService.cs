using ChatApp.Context.EntityClasses;
using ChatApp.Models.UsersModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApp.Business.ServiceInterfaces
{
    public interface IProfileService
    {
        string CheckPassword(LoginModel loginModel);

        string RegisterUser(RegisterModel regModel);

        UpdateModel GetUser(string username);

        public string GenerateJSONWebToken(Profile profileInfo);

        Profile UpdateUser(UpdateModel updateModel, string userName);

        public Profile FetchUsers(string username);
    }
}
