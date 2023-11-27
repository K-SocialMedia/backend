using ChatChit.Data;
using ChatChit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ChatChit.Services
{
    public interface ILoginService
    {
        public Task<UserModel?> CheckInformation(string email, string password);
        public string GenerateJSONWebToken(UserModel user);
    }

    public class LoginService : ILoginService
    {
        private readonly ChatChitContext _context;
        private readonly IConfiguration _config;

        public LoginService(ChatChitContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<UserModel?> CheckInformation(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return null;
            }
            if (BC.Verify(password, user.password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public string GenerateJSONWebToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim("userId", user.id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.fullName),
                    new Claim(JwtRegisteredClaimNames.Email,user.email),
                };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
