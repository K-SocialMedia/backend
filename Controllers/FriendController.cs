using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        public async Task<IActionResult> HandleFriend([FromBody] HandleFriendRequestModel model )
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(model.jwtToken) as JwtSecurityToken;
            if (token != null)
            {
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    FriendModel newFriend = new FriendModel();
                    newFriend.userId = Guid.Parse(userIdClaim.Value);
                    newFriend.friendId = model.friendId;
                    newFriend.status = model.status;
                    await _friendService.HandleFriend(newFriend);
                    return Ok("Status: " + newFriend.status);
                }
            }
            // Token không hợp lệ
            return Unauthorized(); 
        }
    }
}
