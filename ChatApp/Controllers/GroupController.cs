using ChatApp.Business.ServiceInterfaces;
using ChatApp.Context;
using ChatApp.Models.GroupModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly ArgusChatContext context;
        public GroupController(IGroupService groupService, ArgusChatContext context)
        {
            _groupService = groupService;
            this.context = context;
        }

        [HttpPost("CreateGroup")]
        public IActionResult CreateGroup([FromForm] CreateGroupModel createGroup, [FromQuery] string username)
        {

            if (createGroup.GroupName != null)
            {
                return Ok(_groupService.CreateGroup(username, createGroup));
            }
            return BadRequest();
        }

        /*[HttpGet("RecentGroup")]
        public IActionResult RecentChat([FromBody] string username)
        {


            IEnumerable<RecentGroupModel> recentGroups = _groupService.GetRecentGroups(username);
            if (recentGroups != null)
            {
                return Ok(recentGroups);
            }
            return BadRequest();
        }*/

        [HttpPost("GetGroup")]
        public IActionResult GetGroupDetail([FromQuery] int groupId, [FromQuery] string username)
        {

            if (context.Groups.Any(u => u.Id == groupId))
            {
                return Ok(_groupService.getGroup(groupId, username));
            }
            return BadRequest();
        }

        [HttpPost("AllProfiles")]
        public IActionResult GetAllProfiles([FromQuery] int groupId, [FromQuery] string username)
        {

            if (context.Groups.Any(u => u.Id == groupId))
            {
                return Ok(_groupService.getAllProfiles(groupId, username));
            }
            return BadRequest();
        }

        [HttpPost("GetAllMembers")]
        public IActionResult GetAllMembers([FromBody] int groupId)
        {
            return Ok(_groupService.getAllMembers(groupId));
        }

        [HttpPost("AddMemberToGroup/{grpId}")]
        public IActionResult AddMemberToGroup(int grpId, [FromBody] string[] selUsers, [FromQuery] string username)
        {

            if (selUsers != null)
            {
                return Ok(_groupService.addMembersToGroup(grpId, selUsers, username));
            }
            return BadRequest();
        }

        [HttpPost("UpdateGroup/{grpId}")]
        public IActionResult UpdateGroup(int grpId, [FromForm] CreateGroupModel group, [FromBody] string username)
        {

            if (group.GroupName != null && username != null)
            {
                return Ok(_groupService.updateGroup(username, group, grpId));
            }
            return BadRequest();
        }

        [HttpPost("LeaveGroup")]
        public IActionResult LeaveGroup([FromQuery] int groupId, [FromQuery] string username)
        {

            if (groupId != null)
            {
                _groupService.leaveGroup(username, groupId);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("MakeAdmin/{groupId}")]
        public IActionResult MakeAdmin(int groupId, [FromForm] string[] selUserName, [FromQuery] string username)
        {

            if (selUserName != null && username != null)
            {
                _groupService.makeAdmin(groupId, selUserName[0], username);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("RemoveUser/{groupId}")]
        public IActionResult RemoveUser(int groupId, [FromForm] string[] selUserName, [FromQuery] string username)
        {

            if (selUserName != null && username != null)
            {
                _groupService.removeUser(groupId, selUserName[0], username);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("SendFileMessage")]
        public IActionResult SaveFileMessage([FromForm] GroupInputMessageModel msg)
        {
            string filetype = msg.File.ContentType.Split('/')[0];
            if (filetype == "audio" || filetype == "image" || filetype == "video")
            {
                _groupService.SendFileMessage(msg);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("GetAllMessage")]
        public IActionResult GetMessage([FromBody] int groupId)
        {
            IEnumerable<GroupOutputMessageModel> msgList = _groupService.GetAllMessage(groupId);
            if (msgList != null)
            {
                return Ok(msgList);
            }
            return Ok();
        }

    }
}
