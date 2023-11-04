using ChatChit.Models;
using ChatChit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatChit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login.email != null && login.password != null && login.email != "" && login.password != "")
            {
                var user = await _loginService.CheckInformation(login.email, login.password);
                if (user != null)
                {

                }
                else
                {
                    var result = new
                    {
                        auth = false,
                        message = "Wrong username/password!",
                    };
                    return NotFound(result);
                }
            }
            else
            {
                var result = new
                {
                    auth = false,
                    message = "Empty email/password"
                };
                return NotFound(result);
            }
        }
    }
}
