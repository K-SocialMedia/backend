using ChatChit.Helpers;
using ChatChit.Models.RequestModel;
using ChatChit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageGroupController : ControllerBase
    {
        private readonly IMessageGroupService _messageGroupService;
        public MessageGroupController(IMessageGroupService messageGroupService)
        {
            _messageGroupService = messageGroupService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] GroupWithUserRequestModel model)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                if (model.usersId == null)
                {
                    return BadRequest("Danh sách người dùng rỗng");
                }
                else
                {
                    model.usersId = model.usersId.Append(currentUserId).ToArray();
                }
                var result = await _messageGroupService.AddGroup(model);
                if (result.isSuccess == true)
                {
                    return Ok(result.message);
                }
                return BadRequest(result.message);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var result = await _messageGroupService.GetGroupsForUser(currentUserId);
                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(new {message = "Không có group chat"});
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpGet]
        [Route("get-message")]
        public async Task<IActionResult> GetMessage(Guid groupId)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var messages = await _messageGroupService.GetGroupMessages(currentUserId, groupId);
                if (messages == null || messages.Count == 0)
                {
                    return NotFound(new { message = "Chưa có tin nhắn" });
                }
                return Ok(messages);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }
    }
}
