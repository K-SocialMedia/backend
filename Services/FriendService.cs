using ChatChit.Data;
using ChatChit.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IFriendService
    {
        public Task<List<UserModel>> GetAllFriendOfUser(Guid id);
        public Task HandleFriend(FriendModel friend);
    }

    public class FriendService : IFriendService
    {
        private readonly ChatChitContex _contex;
        public FriendService(ChatChitContex contex)
        {
            _contex = contex;
        }

        public async Task<List<UserModel?>> GetAllFriendOfUser(Guid userId)
        {
            var result = await _contex.Friends
                .Where(f => (f.userId == userId || f.friendId == userId) && f.status == FriendModel.FriendStatus.Accepted)
                .Select(f => f.userId == userId ? f.Friend : f.User)
                .ToListAsync();
            return result;
        }

        public async Task HandleFriend(FriendModel friend)
        {
            var check = await _contex.Friends
                        .FirstOrDefaultAsync(f =>
                            (f.userId == friend.userId && f.friendId == friend.friendId) ||
                            (f.userId == friend.friendId && f.friendId == friend.userId)
                        );
            //Nếu đã có mối quan hệ bạn bè trong database thì sẽ thay đổi
            //Nếu chưa có thì sẽ là pending
            if(check != null )
            {
                check.status = friend.status;
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
