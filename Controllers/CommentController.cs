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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Route("get-comment")]
        public async Task<IActionResult> GetComment([FromBody] Guid postId)
        {
            var comments = await _commentService.GetComment(postId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound(new {message = "Chưa có comment"});
            }return Ok(comments);
        }

        [HttpPost]
        [Route("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequestModel model)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var result = await _commentService.AddComment(currentUserId, model);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Bài viết đã bị xóa");
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }
    }
}
