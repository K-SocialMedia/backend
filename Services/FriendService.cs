using ChatChit.Data;
using ChatChit.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IFriendService
    {
        public Task<List<FriendModel>> GetAllFriendOfUser(string id);
    }

    public class FriendService : IFriendService
    {
        private readonly ChatChitContex _contex;
        public FriendService(ChatChitContex contex)
        {
            _contex = contex;
        }

        public async Task<List<FriendModel>> GetAllFriendOfUser(string id)
        {
            var result = await _contex.Friends
                .Where(friend => friend.userId == id)
                .Select(friend => new FriendModel { userId = friend.userId, friendId = friend.friendId })
                .ToListAsync();
            return result;
        }
    }
}
