using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Repositories
{
    public interface IUserService
    {
        public Task<List<UserModel>> GetAllUser();
        public Task<UserModel> GetUserById(Guid currentUserId);
        public Task<List<UserResponseModel>> GetUserByNickName(Guid currentUserId,string nickName);
        public Task<UserModel> AddUser(UserModel user);
        public Task<UserModel?> UpdateUser(Guid currentUserId, UserRequestModel user);
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

        public async Task<UserModel> GetUserById(Guid currentUserId)
        {
            var user = await _context.Users.FindAsync(currentUserId);
            return user;
        }

        public async Task<List<UserResponseModel>> GetUserByNickName(Guid currentUserId, string nickName)
        {
            var users = await _context.Users
            .Where(u => u.id != currentUserId && u.nickName.ToLower().Contains(nickName.ToLower()))
            .Take(15)
            .Select(u => new UserResponseModel
            {
           // Gán thông tin cơ bản từ người dùng
               id = u.id,
               nickName = u.nickName,
               fullName = u.fullName,
               image = u.image,

               isFriend = !_context.Friends.Any(f =>
                   (f.userId == currentUserId && f.friendId == u.id) ||
                   (f.userId == u.id && f.friendId == currentUserId)
               )
            })
            .ToListAsync();

            return users;
        }

        public async Task<UserModel> AddUser(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<UserModel?> UpdateUser(Guid currentUserId, UserRequestModel user)
        {
            var updateUser = await _context.Users.FindAsync(currentUserId);
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
