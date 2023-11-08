using ChatChit.Data;
using ChatChit.Models;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            UserModel findUser = await _userService.GetUser(id);
            if (findUser == null)
            {
                return NotFound();
            }
            return Ok(findUser);
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
    }
}
