using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context;
using ChatApp.Context.EntityClasses;
using ChatApp.Models.MessageModel;
using ChatApp.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Data;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;
        private readonly ChatAppContext context;

        public ChatController(IChatService chatService, ChatAppContext context)
        {
            this.chatService = chatService;
            this.context = context;
        }

        [HttpPost("Addmsg")]
        public IActionResult Addmsg([FromBody] TextMessageModel message)
        {
            if (ModelState.IsValid)
            {
                if (message == null)
                {
                    return BadRequest(new { Message = "Cannot send a empty message." });
                }
                chatService.SendMessage(message);
                return Ok(new { Message = "message added succesfully." });
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }

        [HttpPost("AddFilemsg")]
        public IActionResult AddFilemsg([FromForm] FileMessageModel message)
        {
            if (ModelState.IsValid)
            {
                if (message == null)
                {
                    return BadRequest(new { Message = "Cannot send a empty message." });
                }
                chatService.SendFileMessage(message);
                return Ok(new { Message = "File message added succesfully." });
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }


        [HttpPost("Addreplymsg")]
        public IActionResult AddReplymsg([FromForm] TextMessageModel message, [FromQuery] int messageId)
        {
            if (ModelState.IsValid)
            {
                if (message == null)
                {
                    return BadRequest(new { Message = "Cannot send a empty message." });
                }
                chatService.ReplyMessage(message, messageId);
                return Ok(new { Message = "message added succesfully." });
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }


        [HttpGet("GetMessages")]

        public async Task<IActionResult> Getmsgs([FromQuery] string username, [FromQuery] string selusername)
        {
           

            if (ModelState.IsValid)
            {
                var MessageList = chatService.GetMessages(username, selusername);
                if (MessageList == null)
                {
                    return Ok(new { Message = "No Messages" });
                }
                return Ok(MessageList);
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }

        // Method to search other users across the chatapp using their username 

        [HttpGet("SearchOthers")]

        public IActionResult GetOtherUsers([FromQuery] string searchname, [FromQuery] string username)
        {

            var searchModels = chatService.SearchOthers(searchname, username);

            if (searchModels.IsNullOrEmpty())
            {
                return Ok(new { Message = "No User With Given Name.", searchModels });
            }

            return Ok(searchModels);
        }

        [HttpGet("RecentMessages")]
        
        public IActionResult GetConversations([FromQuery]string userName)
        {
            var usrId = chatService.FetchUserIdByUsername(userName);
            var conversations = context.ConversationResults.FromSqlRaw("EXEC dbo.GetAllConversationByUserId @p0", usrId).ToList();
            // conversations.Reverse();
            return Ok(conversations);


        }

        [HttpGet("UnreadCount")]

        public IActionResult GetUnreadCount([FromQuery] int usrId , [FromQuery] int otherId)
        {

            var conversations = context.ConversationResults.FromSqlRaw("EXEC dbo.GetAllConversationByUserIdsBoth @UserID, @OtherID", new SqlParameter("UserID", usrId), new SqlParameter("OtherID", otherId)).ToList();
            var unreadCount = 0;
            foreach (var conversation in conversations)
            {
               // if(conversation.)
            }
            return Ok(conversations);


        }


        [HttpDelete("DeleteMessage")]

        public IActionResult Delete(int MessageId)
        {
            if (ModelState.IsValid)
            {
                chatService.DeleteMessage(MessageId);
                return Ok();
            }
            return BadRequest(new { Message = "Model State is not valid." });

        }

        [HttpGet("GetUserId")]

        public IActionResult GetUserId(string userName)
        {
            if (ModelState.IsValid)
            {
                var id = chatService.FetchUserIdByUsername(userName);
                return Ok(id);
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }

        // Messages as read or unread 
        [HttpPost("markAsRead")]
        public IActionResult markAsRead([FromForm] string username, [FromForm] string selusername)
        {
            if (ModelState.IsValid)
            {
                chatService.MarkAsRead(username, selusername);
                return Ok(new { Message = "Marked as Read." });
            }
            return BadRequest(new { Message = "Model State is not valid." });
        }

    }
}
