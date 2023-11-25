using ChatChit.Helpers;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using ChatChit.Services;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet]
        [Route("get-all-friend-of-user")]
        public async Task<IActionResult> GetAllFriendOfUser()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var result = await _friendService.GetAllFriendOfUser(currentUserId);
                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Dont have any friend");
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpGet]
        [Route("get-all-pending-friend-of-user")]
        public async Task<IActionResult> GetAllPendingFriendOfUser()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var result = await _friendService.GetPendingFriendOfUser(currentUserId);
                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No friend request");
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpPost]
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
            return BadRequest(new { message = "UserId claim not found in token" });
        }
    }
}
