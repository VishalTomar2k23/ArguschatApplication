using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models.UsersModel
{
    // this model will be used for updation of the profile of the image and to add the image to the folder 
    public class UpdateModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; } 
        
        [Required]
        /*[RegularExpression("^([a-z0-9]+@[a-z]+\\.[a-z]{2,3})$", ErrorMessage = "Enter a Valid Email")]*/
        public string Email { get; set; }
        public IFormFile ProfileImage { get; set; }

        public string? ProfileImageLocation { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? Designation { get; set; }
    }
}
