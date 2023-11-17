using ChatChit.Data;
using ChatChit.Models;
using ChatChit.Models.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IFriendService
    {
        public Task<List<UserResponseModel>> GetAllFriendOfUser(Guid currentUserId);
        public Task<List<UserResponseModel>> GetPendingFriendOfUser(Guid currentUserId);
        public Task HandleFriend(FriendModel friend);
    }

    public class FriendService : IFriendService
    {
        private readonly ChatChitContex _contex;
        public FriendService(ChatChitContex contex)
        {
            _contex = contex;
        }

        public async Task<List<UserResponseModel>> GetAllFriendOfUser(Guid currentUserId)
        {
            var result = await _contex.Friends
                .Where(f => (f.userId == currentUserId || f.friendId == currentUserId) 
                && f.status == FriendModel.FriendStatus.Accepted)
                .Select(f => f.userId == currentUserId ? f.Friend : f.User)
                .Select(user => new UserResponseModel
                {
                    id = user.id,
                    nickName = user.nickName,
                    fullName = user.fullName,
                    image = user.image
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<UserResponseModel>> GetPendingFriendOfUser(Guid currentUserId)
        {
            var users = await _contex.Friends
                .Where(f => (f.userId == currentUserId || f.friendId == currentUserId) 
                && f.status == FriendModel.FriendStatus.Pending)
                .Select(f => f.userId == currentUserId ? f.Friend : f.User)
                .Select(user => new UserResponseModel
                {
                    id = user.id,
                    nickName = user.nickName,
                    fullName = user.fullName,
                    image = user.image
                })
                .ToListAsync();

            return users;
        }

        public async Task HandleFriend(FriendModel friend)
        {
            var check = await _contex.Friends
                        .FirstOrDefaultAsync(f =>
                            (f.userId == friend.userId && f.friendId == friend.friendId) ||
                            (f.userId == friend.friendId && f.friendId == friend.userId)
                        );
            if(check != null )
            {
                if(friend.status == FriendModel.FriendStatus.Rejected)
                {
                    _contex.Friends.Remove(check);
                }
                else
                {
                check.status = friend.status;
                }
            }
            else
            {
                friend.status = FriendModel.FriendStatus.Pending;
                _contex.Friends.Add(friend);
            }
            await _contex.SaveChangesAsync();
        }
    }
}
