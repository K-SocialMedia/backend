using ChatChit.Data;
using ChatChit.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ChatChit.Services
{
    public interface ILoginService
    {
        public Task<UserModel> CheckInformation(string email, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly ChatChitContex _context;

        public LoginService(ChatChitContex context)
        {
            _context = context;
        }

        public async Task<UserModel> CheckInformation(string email, string password)
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
    }
}
