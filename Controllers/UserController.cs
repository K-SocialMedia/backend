using ChatChit.Data;
using ChatChit.Helpers;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using ChatChit.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using BC = BCrypt.Net.BCrypt;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get-all-user")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllUser()
        {
            try
            {
                return Ok(await _userService.GetAllUser());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody] CreateUserModel user)
        {
            //var existingUserWithEmail = await _userService.GetUserByEmail(user.email);
            //if (existingUserWithEmail != null)
            //{
            //    return BadRequest(new { message = "Email already exists" });
            //}

            //var existingUserWithNickname = await _userService.GetUserByNickname(user.nickName);
            //if (existingUserWithNickname != null)
            //{
            //    return BadRequest(new { message = "Nickname already exists" });
            //}
            if (!await _userService.CheckUniqueEmail(user.email))
            {
                return BadRequest(new { message = "Email da ton tai" });
            }
            var hashedPassword = BC.HashPassword(user.password);
            UserModel newUser = new UserModel();
            newUser.fullName = user.fullName;
            newUser.email = user.email;
            newUser.password = hashedPassword;
            newUser.nickName = user.nickName;
            newUser.createdAt = DateTime.Now.ToUniversalTime();
            newUser.updatedAt = DateTime.Now.ToUniversalTime();
            await _userService.AddUser(newUser);
            return Ok(newUser);

        }


        [HttpGet]
        [Route("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                UserResponseModel findUser = await _userService.GetUserById(currentUserId, id);
                if (findUser == null)
                {
                    return NotFound(new { message = "Khong tim thay user" });
                }
                return Ok(findUser);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpGet]
        [Route("get-user-information")]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                UserResponseModel findUser = await _userService.GetUserInformation(currentUserId);
                if (findUser == null)
                {
                    return NotFound(new { message = "Khong tim thay user" });
                }
                return Ok(findUser);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }


        [HttpGet]
        [Route("get-user-by-nick-name")]
        public async Task<IActionResult> GetUserByNickName(string nickName)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                List<UserResponseModel> result = await _userService.GetUserByNickName(currentUserId, nickName);
                if (result == null || result.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserRequestModel user)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                var updateUser = await _userService.UpdateUser(currentUserId, user);
                if (updateUser != null)
                {
                    return Ok(updateUser);
                }
                else
                {
                    return NotFound(new { message = "User not found" });
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }

        [HttpPut]
        [Route("change-user-password")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequestModel model)
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                StatusHelper changeResult = await _userService.ChangePassword(currentUserId, model.oldPassword, model.newPassword);
                if (changeResult != null && changeResult.isSuccess == true)
                {
                    return Ok(new { message = changeResult.message });
                }
                else
                {
                    return NotFound(new { message = changeResult.message });
                }
            }
            return BadRequest(new { message = "UserId claim not found in token" });
        }
        [HttpGet]
        [Route("get-related-friend")]
        public async Task<IActionResult> GetRelatedFriend()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                // Gọi phương thức từ service để lấy danh sách người dùng gần đây nhất nhưng chưa kết bạn
                var relatedFriends = await _userService.GetRelatedFriend(currentUserId);
                if(relatedFriends != null)
                {
                return Ok(relatedFriends);
                }return NotFound(new { message = "Chua tim thay ban moi" });
            }

            return BadRequest(new { message = "UserId claim not found in token" });
        }

    }
}
