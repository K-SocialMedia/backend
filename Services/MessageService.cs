using ChatChit.Data;
using ChatChit.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatChit.Services
{
    public interface IMessageService
    {
        public Task<List<MessageModel>> GetMessageNearly(Guid currentUserId, Guid friendId);
    }
    public class MessageService : IMessageService
    {
        private readonly ChatChitContext _context;
        public MessageService(ChatChitContext context)
        {
            _context = context;
        }
        public async Task<List<MessageModel>> GetMessageNearly(Guid currentUserId, Guid friendId)
        {
            var messages = await _context.Messages
                 .Where(m => (m.senderId == currentUserId && m.receiverId == friendId) ||
                   (m.senderId == friendId && m.receiverId == currentUserId))
                .OrderByDescending(m => m.createAt)
                .Take(5)
                 .ToListAsync();

            return messages;
        }
    }
}
