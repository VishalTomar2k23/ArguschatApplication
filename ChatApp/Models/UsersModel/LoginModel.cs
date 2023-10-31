using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models.UsersModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is Required.")]
        public string Username { get; set; }

        [Key]
        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@#$%^&*]{8,}$", ErrorMessage = "Password  is not valid.")]
        public string Password { get; set; }
    }
}
