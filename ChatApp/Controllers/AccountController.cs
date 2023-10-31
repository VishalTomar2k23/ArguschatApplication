using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatApp.Business.Helpers;
using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context.EntityClasses;
using ChatApp.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IProfileService _profileService;
        public AccountController(IConfiguration config, IProfileService profileService)
        {
            _config = config;
            _profileService = profileService;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IActionResult response = Unauthorized(new { Message = "Invalid Credentials." });
                var tokenString = _profileService.CheckPassword(loginModel);

                if (tokenString != null)
                {

                    response = Ok(new { token = tokenString, username = loginModel.Username });
                }

                return response;
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {
            // we can create a custom validation for checking if the email is already registered or not . 
            // we will also check if the state is valid or not. 
            if (ModelState.IsValid)
            {
                var tokenString = _profileService.RegisterUser(registerModel);
                if (tokenString != null)
                {

                    return Ok(new { token = tokenString });
                }
                return BadRequest(new { Message = "User Already Exists. Please use different email and UserName." });
            }
            return BadRequest( new { Message = " this is bad state error.", ModelState});
        }

        // Creating a post method to update user information 
        [Authorize]
        [HttpPost("update-user")]
        public IActionResult UpdateUserProfile([FromForm] UpdateModel updateModel, [FromHeader] string UserName)
        {
            if (ModelState.IsValid)
            {
                if (updateModel.UserName == UserName)
                {
                    Profile user = _profileService.UpdateUser(updateModel, UserName);
                    return Ok(new { Message = " Updated the user", token = user });
                }
                return BadRequest(new { Message = "Unable to Update the user . Try again with correct information", ModelState });
            }
            return BadRequest(ModelState);

        }

        [Authorize]
        [HttpGet("get-user")]

        public IActionResult GetUserProfile([FromQuery] string UserName)
        {
            if (ModelState.IsValid)
            {
                var user = _profileService.GetUser
                    (UserName);
                return Ok(user);
            }
            return BadRequest(ModelState);
        }




    }
}