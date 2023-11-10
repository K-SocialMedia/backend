using ChatChit.Models;
using ChatChit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using static Npgsql.PostgresTypes.PostgresCompositeType;

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
        public async Task<IActionResult> GetFriendOfUser(Guid id)
        {
                var result = await _friendService.GetAllFriendOfUser(id);
                return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> HandleFriend(Guid myId, Guid friendId, FriendModel.FriendStatus status)
        {
            try
            {
                FriendModel newFriend = new FriendModel();
                newFriend.userId = myId;
                newFriend.friendId = friendId;
                newFriend.status = status;
                await _friendService.HandleFriend(newFriend);
                return Ok("Status: " + newFriend.status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
