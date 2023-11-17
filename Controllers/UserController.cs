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
        [Route("get-user-by-id")]
        public async Task<IActionResult> GetUserById()
        {
            var userId = TokenHelper.GetUserIdFromClaims(User);
            if (userId != null)
            {
                Guid currentUserId = userId.Value;
                UserModel findUser = await _userService.GetUserById(currentUserId);
                if (findUser == null)
                {
                    return NotFound();
                }
                return Ok(findUser);
            }
            return BadRequest("UserId claim not found in token");
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
            return BadRequest("UserId claim not found in token");
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
                    return NotFound("User not found");
                }
            }
            return BadRequest("UserId claim not found in token");
        }
    }
}
