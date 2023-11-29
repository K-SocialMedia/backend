using ChatChit.Helpers;
using ChatChit.Models;
using ChatChit.Models.ResponseModel;
using ChatChit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> GetMessage([FromBody] Guid id)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var messages = await _messageService.GetMessageNearly(currentUserId, id);
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
