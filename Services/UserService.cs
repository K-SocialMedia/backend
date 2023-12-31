﻿using ChatChit.Data;
using ChatChit.Helpers;
using ChatChit.Models;
using ChatChit.Models.RequestModel;
using ChatChit.Models.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using BC = BCrypt.Net.BCrypt;

namespace ChatChit.Repositories
{
    public interface IUserService
    {
        public Task<List<UserModel>> GetAllUser();
        public Task<UserResponseModel> GetUserById(Guid currentUserId, Guid id);
        public Task<List<UserResponseModel>> GetUserByNickName(Guid currentUserId, string nickName);
        public Task<List<UserResponseModel>> GetRelatedFriend(Guid id);
        public Task<UserModel> AddUser(UserModel user);
        public Task<UserModel?> UpdateUser(Guid currentUserId, UserRequestModel user);
        public Task DeleteUser(UserModel deleteUser);
        public Task<StatusHelper> ChangePassword(Guid currentUserId, string oldPassword, string newPassword);
        public Task<bool> CheckUniqueEmail(string email);
        public Task<UserResponseModel> GetUserInformation(Guid currentUserId);
    }

    public class UserService : IUserService
    {
        private readonly ChatChitContext _context;

        public UserService(ChatChitContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<UserResponseModel> GetUserById(Guid currentUserId, Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.id == currentUserId)
            {
                return null;
            }

            var status = _context.Friends
                .Any(f =>
                    (f.userId == currentUserId && f.friendId == user.id) ||
                    (f.userId == user.id && f.friendId == currentUserId)
                )
                ? _context.Friends
                    .Where(f =>
                        (f.userId == currentUserId && f.friendId == user.id) ||
                        (f.userId == user.id && f.friendId == currentUserId)
                    )
                    .Select(f => f.status)
                    .FirstOrDefault()
                : FriendModel.FriendStatus.Rejected;

            var userResponse = new UserResponseModel
            {
                id = user.id,
                nickName = user.nickName,
                fullName = user.fullName,
                image = user.image,
                status = status ?? FriendModel.FriendStatus.Rejected
            };

            return userResponse;
        }

        public async Task<UserResponseModel> GetUserInformation(Guid currentUserId)
        {
            var user = await _context.Users.FindAsync(currentUserId);
            if (user == null)
            {
                return null;
            }

            var userResponse = new UserResponseModel
            {
                id = user.id,
                nickName = user.nickName,
                fullName = user.fullName,
                image = user.image,
                email = user.email,
            };

            return userResponse;
        }

        public async Task<List<UserResponseModel>> GetUserByNickName(Guid currentUserId, string nickName)
        {
            var users = await _context.Users
                .Where(u => u.id != currentUserId && u.nickName.ToLower().Contains(nickName.ToLower()))
                .Take(15)
                .Select(u => new UserResponseModel
                {
                    id = u.id,
                    nickName = u.nickName,
                    fullName = u.fullName,
                    image = u.image,

                    status = _context.Friends
                        .Where(f =>
                            (f.userId == currentUserId && f.friendId == u.id) ||
                            (f.userId == u.id && f.friendId == currentUserId)
                        )
                        .Select(f => f.status)
                        .FirstOrDefault() ?? FriendModel.FriendStatus.Rejected
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

        public async Task<StatusHelper> ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            UserModel user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (BC.Verify(oldPassword, user.password))
                {
                    user.password = BC.HashPassword(newPassword);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    return new StatusHelper { isSuccess = true, message = "Mật khẩu đã được thay đổi thành công" };
                }
                else
                {
                    return new StatusHelper { isSuccess = false, message = "Mật khẩu cũ không chính xác" };
                }
            }

            return new StatusHelper { isSuccess = false, message = "Không tìm thấy người dùng" };
        }
        public async Task<List<UserResponseModel>> GetRelatedFriend(Guid currentUserId)
        {
            var users = await _context.Users
                .Where(u => u.id != currentUserId &&
                            !_context.Friends.Any(f =>
                                (f.userId == currentUserId && f.friendId == u.id) ||
                                (f.userId == u.id && f.friendId == currentUserId)
                            ))
                .OrderByDescending(u => u.createdAt)
                .Take(10)
                .Select(u => new UserResponseModel
                {
                    id = u.id,
                    nickName = u.nickName,
                    fullName = u.fullName,
                    image = u.image,
                    status = FriendModel.FriendStatus.Rejected
                })
                .ToListAsync();

            return users;
        }

        public async Task<bool> CheckUniqueEmail(string email)
        {
            var userWithEmail = await _context.Users.FirstOrDefaultAsync(u => u.email == email);

            return userWithEmail == null;
        }
    }
}
