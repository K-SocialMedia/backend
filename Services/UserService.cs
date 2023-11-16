using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Repositories
{
    public interface IUserService
    {
        public Task<List<UserModel>> GetAllUser();
        public Task<UserModel> GetUserById(Guid id);
        public Task<List<UserModel>> GetUserByNickName(string nickName);
        public Task<UserModel> AddUser(UserModel user);
        public Task<UserModel?> UpdateUser(UserRequestModel user, Guid id);
        public Task DeleteUser(UserModel deleteUser);
    }

    public class UserService : IUserService
    {
        private readonly ChatChitContex _context;

        public UserService(ChatChitContex context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<UserModel> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<List<UserModel>> GetUserByNickName(string nickName)
        {
            var newNickName = nickName.ToLower();
            var users = await _context.Users
        .Where(u => u.nickName.ToLower().Contains(newNickName))
        .Take(5)
        .ToListAsync();
            return users;
        }

        public async Task<UserModel> AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel?> UpdateUser(UserRequestModel user, Guid id)
        {
            var updateUser = await _context.Users.FindAsync(id);
            if (updateUser != null)
            {
                if (user.fullName != null)
                    updateUser.fullName = user.fullName;

                if (user.image != null)
                    updateUser.image = user.image;

                if (user.nickName != null)
                    updateUser.nickName = user.nickName;

                updateUser.updatedAt = DateTime.UtcNow;

                _context.Entry(updateUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return updateUser;
            }
            return null;
        }

        public async Task DeleteUser(UserModel deleteUser)
        {
            _context.Users.Remove(deleteUser);
            await _context.SaveChangesAsync();
        }
    }
}
