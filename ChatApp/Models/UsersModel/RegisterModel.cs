using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models.UsersModel
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "UserName is Required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d!@#$%^&*]{8,}$", ErrorMessage = "Password  is not valid.")]
        public string Password { get; set; }
    }
}
