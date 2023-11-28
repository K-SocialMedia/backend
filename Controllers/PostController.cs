using ChatChit.Helpers;
using ChatChit.Models.Post;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using ChatChit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPostById()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                List<PostModel> posts = await _postService.GetAllPostById(currentUserId);
                if (posts != null)
                {
                    return Ok(posts);
                }
                return NotFound(new { message = "Khong tim thay post" });
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostRequestModel post)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                PostModel model = await _postService.AddPost(currentUserId, post);
                return Ok(model);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePost([FromBody] Guid postId)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                if(await _postService.DeletePost(currentUserId, postId))
                {
                    return Ok(new {message = "Xoa thanh cong"});
                }return BadRequest(new { message = "Xoa that bai" });
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }
    }
}
