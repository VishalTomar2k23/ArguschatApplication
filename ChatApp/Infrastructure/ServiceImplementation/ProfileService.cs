using ChatApp.Business.Helpers;
using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context;
using ChatApp.Context.EntityClasses;
using ChatApp.Models.UsersModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Infrastructure.ServiceImplementation
{
    public class ProfileService : IProfileService
    {
        private readonly ChatAppContext context;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration _config;

        public ProfileService(ChatAppContext context, IWebHostEnvironment environment, IConfiguration config)
        {
            this.context = context;
            this.environment = environment;
            _config = config;
        }
        public string CheckPassword(LoginModel model)
        {
            var user = this.context.Profiles.FirstOrDefault(x => model.Password == x.Password
            && (x.Email.ToLower().Trim() == model.EmailAddress.ToLower().Trim() || x.UserName.ToLower().Trim() == model.Username.ToLower().Trim()));
            if (user == null) { return null; }
            return GenerateJSONWebToken(user);
        }

        public string RegisterUser(RegisterModel regModel)
        {
            Profile newUser = null;
            if (!CheckEmailOrUserNameExists(regModel.UserName, regModel.Email))
            {
                newUser = new Profile
                {
                    FirstName = regModel.FirstName,
                    LastName = regModel.LastName,
                    Password = regModel.Password,
                    UserName = regModel.UserName,
                    Email = regModel.Email,
                    CreatedAt = DateTime.UtcNow,
                    ProfileType = (int)ProfileType.User,
                    // we will add image path for default image  
                    ImagePath = "/Images/default.png"
                };
                context.Profiles.Add(newUser);
                context.SaveChanges();
            }
            return GenerateJSONWebToken(newUser);
        }

        public string GenerateJSONWebToken(Profile profileInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, profileInfo.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, profileInfo.Email),
                    new Claim(ClaimsConstant.FirstNameClaim, profileInfo.FirstName),
                    new Claim(ClaimsConstant.LastNameClaim, profileInfo.LastName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimsConstant.ImagePathClaim, profileInfo.ImagePath)

        };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool CheckEmailOrUserNameExists(string userName, string email)
        {
            return context.Profiles.Any(x => x.Email.ToLower().Trim() == email.ToLower().Trim() || x.UserName.ToLower().Trim() == userName.ToLower().Trim());
        }

        public UpdateModel GetUser(string username)
        {
            // we will just perform a simple search for username across database 
            // It may return a null value so we will check the value before using it somewhere
            var profile =  (context.Profiles.FirstOrDefault( x => x.UserName.ToLower().Trim() == username.ToLower().Trim()));
            if (profile == null)
            {
                return null;
            }
            var user = new UpdateModel()
            {
                UserName = profile.UserName,
                Email = profile.Email,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                ProfileImageLocation = profile.ImagePath,
                LastUpdatedAt = profile.LastUpdatedAt,
            };
            return user;
        }

        public Profile FetchUsers(string username)
        {
            // we will just perform a simple search for username across database 
            // It may return a null value so we will check the value before using it somewhere
            return (context.Profiles.FirstOrDefault(x => x.UserName.ToLower().Trim() == username.ToLower().Trim()));
        }
        public int FetchUserIdByUsername(string username)
        {   
            Profile user = context.Profiles.FirstOrDefault(profile => profile.UserName == username);
            return user.Id;
        }

        public Profile UpdateUser(UpdateModel updateModel, string userName)
        {
            // first we will have to import data from UpdateModel
            Profile updateUser = FetchUsers(updateModel.UserName);

            // we don't have that user in our database so we will delete it.
            if (updateUser == null) {
                return null; // me
            }
            // Our username from frontent and backend don't match
            if (updateUser.Email == context.Profiles.FirstOrDefault().Email && updateUser.UserName != userName)
            {
                return null; // 
            }

            /*string Filepath = this.environment.WebRootPath + */
            
            // To Upload the image 
            if(updateUser.ImagePath != null)
            {
                // First we will create a image name using Guid
                string FileName = Guid.NewGuid().ToString();
                // 

                var uploads = Path.Combine(environment.WebRootPath, @"Images/Users/");

                // Find the extension of the file and also check if it is a image file or not  Path.GetExtension(updateModel.ProfileImageLocation);
                var extension = System.IO.Path.GetExtension(updateModel.ProfileImage.FileName);// To Extract the extension from the imagepath


                string[] allowedExtensions = new string[] { ".jpeg", ".png", ".jpg" };
                if (!allowedExtensions.Contains(extension))
                {
                    return updateUser; // returning previous data from profile of the user

                }

                //If image is stored then we will first delete it
                if (updateUser.ImagePath != null && !updateUser.ImagePath.Equals("/Images/default.png"))
                {
                    var oldImagePath = Path.Combine(environment.WebRootPath + updateUser.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Now we will create a fileStream 
                using (var fileStreams = new FileStream(Path.Combine(uploads+FileName+extension), FileMode.Create))
                {
                   updateModel.ProfileImage.CopyTo(fileStreams);
                }
                
                // add the new image saved path in profile entity 
                updateUser.ImagePath = environment.WebRootPath + "/Images/Users/"+FileName+extension;

            }

            updateUser.FirstName = updateModel.FirstName;
            updateUser.LastName = updateModel.LastName;
            updateUser.Email = updateModel.Email;
            updateUser.LastUpdatedAt = DateTime.Now;
            // create a method to get userId by username
            updateUser.LastUpdatedBy = FetchUserIdByUsername(updateUser.UserName);
            context.Profiles.Update(updateUser);
            context.SaveChanges();
            return updateUser;
        }
    }
}
