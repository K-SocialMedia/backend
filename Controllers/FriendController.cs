using ChatChit.Helpers;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using ChatChit.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet]
        [Route("get-all-friend-of-user")]
        [Authorize]
        public async Task<IActionResult> GetAllFriendOfUser()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid id = userId.Value;
                var users = await _friendService.GetAllFriendOfUser(id);
                List<UserResponse> result = users.Select(user => new UserResponse
                {
                    id = user.id,
                    nickName = user.nickName,
                    fullName = user.fullName,
                    image = user.image
                }).ToList();
                return Ok(result);
            }
            return BadRequest("UserId claim not found in token");
        }

        [HttpGet]
        [Route("get-all-pending-friend-of-user")]
        [Authorize]
        public async Task<IActionResult> GetAllPendingFriendOfUser()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid id = userId.Value;
                var users = await _friendService.GetPendingFriendOfUser(id);
                List<UserResponse> result = users.Select(user => new UserResponse
                {
                    id = user.id,
                    nickName = user.nickName,
                    fullName = user.fullName,
                    image = user.image
                }).ToList();
                return Ok(result);
            }
            return BadRequest("UserId claim not found in token");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> HandleFriend([FromBody] HandleFriendRequestModel model )
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid id = userId.Value;
                FriendModel newFriend = new FriendModel();
                newFriend.userId = id;
                newFriend.friendId = model.friendId;
                newFriend.status = model.status;
                await _friendService.HandleFriend(newFriend);
                return Ok("Status: " + newFriend.status);
            };
            // Token không hợp lệ
            return Unauthorized(); 
        }
    }
}
