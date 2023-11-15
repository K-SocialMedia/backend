using ChatChit.Data;
using ChatChit.Helpers;
using ChatChit.Models;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get-all-user")]
        public async Task<IActionResult> getAllUser()
        {
            try
            {
                //Postman test ok

                //var currentUser = HttpContext.User;
                //string authorizationHeader = HttpContext.Request.Headers["Authorization"];
                //string token = authorizationHeader.Substring("Bearer ".Length);

                //var tokenHandler = new JwtSecurityTokenHandler();
                //var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                //if (securityToken != null)
                //{
                //    var nameClaim = securityToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);

                //    if (nameClaim != null)
                //    {
                //        return Ok(nameClaim.Value);
                //    }
                //}

                //Test not ok

                //var nameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);

                //if (nameClaim != null)
                //{
                //    return Ok(nameClaim.Value);
                //}
                return Ok(await _userService.GetAllUser());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
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
                Guid id = userId.Value;
                UserModel findUser = await _userService.GetUserById(id);
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
        [Authorize]
        public async Task<IActionResult> GetUserByNickName(string nickName)
        {
            List<UserModel> users = await _userService.GetUserByNickName(nickName);
            if (users == null)
            {
                return NotFound();
            }

            List<UserResponse> result = users.Select(user => new UserResponse
            {
                id = user.id,
                nickName = user.nickName,
                fullName = user.fullName,
                image = user.image
            }).ToList();

            return Ok(result);
        }
    }
}
